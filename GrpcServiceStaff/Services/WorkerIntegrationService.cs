using Common;
using Common.Models.DB;
using Grpc.Core;
using GrpcServiceStaff.Data.Repo;

namespace GrpcServiceStaff.Services
{
    public class WorkerIntegrationService : WorkerIntegration.WorkerIntegrationBase
    {
        private readonly ILogger<WorkerIntegrationService> _logger;
        private readonly IRepository<Employee> _employeeRepository;

        public WorkerIntegrationService(
            ILogger<WorkerIntegrationService> logger,
            IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public override async Task GetWorkerStream(
            EmptyMessage request,
            IServerStreamWriter<WorkerAction> responseStream,
            ServerCallContext context)
        {
            try
            {
                var items = await _employeeRepository.Get();
                foreach (var employee in items)
                {
                    await responseStream.WriteAsync(
                        new WorkerAction() { ActionType = Action.Default, Worker = employee.ToWorkerMessage() });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex?.Message}"); ;
            }
        }

        public override async Task<WorkerMessage?> DoWorkerAction(WorkerAction request, ServerCallContext context)
        {
            try
            {
                Employee? employee = null;
                if (request?.Worker != null)
                {
                    switch (request.ActionType)
                    {
                        case Action.Create:
                            employee = await _employeeRepository.Create(request.Worker.ToEmployee());
                            break;
                        case Action.Update:
                            employee = await _employeeRepository.Update(request.Worker.ToEmployee());
                            break;
                        case Action.Delete:
                            employee = await _employeeRepository.Delete(request.Worker.ToEmployee());
                            break;
                    }
                }
                return employee?.ToWorkerMessage();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex?.Message}");
                return null;
            }
        }
    }
}
