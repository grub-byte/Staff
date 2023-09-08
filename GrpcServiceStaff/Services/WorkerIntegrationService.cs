using Grpc.Core;

namespace GrpcServiceStaff.Services
{
    public class WorkerIntegrationService : WorkerIntegration.WorkerIntegrationBase
    {
        private readonly ILogger<WorkerIntegrationService> _logger;
        public WorkerIntegrationService(ILogger<WorkerIntegrationService> logger)
        {
            _logger = logger;
        }

        public override Task GetWorkerStream(EmptyMessage request, IServerStreamWriter<WorkerAction> responseStream, ServerCallContext context)
        {
            return base.GetWorkerStream(request, responseStream, context);
        }
    }
}
