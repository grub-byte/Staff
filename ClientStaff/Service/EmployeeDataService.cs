using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Common.Models.Client;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServiceStaff;
using NLog;
using Action = GrpcServiceStaff.Action;

namespace ClientStaff.Service
{
    internal class EmployeeDataService : IDataService<ClientEmployee>
    {
        #region Private Fields
        private readonly WorkerIntegration.WorkerIntegrationClient _client;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Ctor
        public EmployeeDataService()
        {
            var channel = GrpcChannel.ForAddress(Properties.Settings.Default.ApiServer);
            _client = new WorkerIntegration.WorkerIntegrationClient(channel);
        }
        #endregion

        #region IDataService<Employee>
        public async Task<ClientEmployee?> Create(ClientEmployee item)
        {
            try
            {
                return await DoWorkerAction(item, Action.Create);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        public async Task<ClientEmployee?> Delete(ClientEmployee item)
        {
            try
            {
                return await DoWorkerAction(item, Action.Delete);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        public async IAsyncEnumerable<ClientEmployee> Get()
        {
            var serverData = _client.GetWorkerStream(new EmptyMessage());
            var responseStream = serverData.ResponseStream;
            await foreach (var workerAction in responseStream.ReadAllAsync())
            {
                var employee = workerAction.Worker.ToClientEmployee();
                yield return employee;
            }
        }

        public async Task<ClientEmployee?> Update(ClientEmployee item)
        {
            try
            {
                return await DoWorkerAction(item, Action.Update);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
        #endregion

        #region Private Methods
        private async Task<ClientEmployee?> DoWorkerAction(ClientEmployee item, Action action)
        {
            var employee = item.ToWorkerMessage();
            var wm = await _client.DoWorkerActionAsync(new WorkerAction() { ActionType = action, Worker = employee });
            return wm?.ToClientEmployee();
        }
        #endregion
    }
}
