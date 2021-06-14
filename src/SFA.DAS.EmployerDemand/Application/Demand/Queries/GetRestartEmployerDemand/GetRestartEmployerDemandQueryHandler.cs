using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetRestartEmployerDemand
{
    public class GetRestartEmployerDemandQueryHandler : IRequestHandler<GetRestartEmployerDemandQuery, GetRestartEmployerDemandQueryResult>
    {
        private readonly IEmployerDemandApiClient<EmployerDemandApiConfiguration> _demandApiClient;

        public GetRestartEmployerDemandQueryHandler (IEmployerDemandApiClient<EmployerDemandApiConfiguration> demandApiClient)
        {
            _demandApiClient = demandApiClient;
        }
        public async Task<GetRestartEmployerDemandQueryResult> Handle(GetRestartEmployerDemandQuery request, CancellationToken cancellationToken)
        {
            var expiredDemandTask =
                _demandApiClient.Get<GetEmployerDemandResponse>(
                    new GetEmployerDemandByExpiredDemandRequest(request.Id));
            
            var demandTask = 
                _demandApiClient.Get<GetEmployerDemandResponse>(
                    new GetEmployerDemandRequest(request.Id));

            await Task.WhenAll(expiredDemandTask, demandTask);

            if (expiredDemandTask.Result != null)
            {
                return new GetRestartEmployerDemandQueryResult
                {
                    EmployerDemand = expiredDemandTask.Result,
                    RestartDemandExists = true
                };
            }

            return new GetRestartEmployerDemandQueryResult
            {
                EmployerDemand = demandTask.Result,
                RestartDemandExists = false
            };
        }
    }
}