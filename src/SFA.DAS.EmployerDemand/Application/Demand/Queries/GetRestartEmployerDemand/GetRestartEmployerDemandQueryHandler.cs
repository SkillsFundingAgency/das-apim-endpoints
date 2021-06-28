using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetRestartEmployerDemand
{
    public class GetRestartEmployerDemandQueryHandler : IRequestHandler<GetRestartEmployerDemandQuery, GetRestartEmployerDemandQueryResult>
    {
        private readonly IEmployerDemandApiClient<EmployerDemandApiConfiguration> _demandApiClient;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public GetRestartEmployerDemandQueryHandler (IEmployerDemandApiClient<EmployerDemandApiConfiguration> demandApiClient, ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _demandApiClient = demandApiClient;
            _coursesApiClient = coursesApiClient;
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

            var coursesResult = await _coursesApiClient.Get<GetStandardsListItem>(new GetStandardRequest(demandTask.Result.Course.Id));

            if (expiredDemandTask.Result != null)
            {
                return new GetRestartEmployerDemandQueryResult
                {
                    EmployerDemand = expiredDemandTask.Result,
                    RestartDemandExists = true,
                    LastStartDate = coursesResult.StandardDates.LastDateStarts
                };
            }

            return new GetRestartEmployerDemandQueryResult
            {
                EmployerDemand = demandTask.Result,
                RestartDemandExists = false,
                LastStartDate = coursesResult.StandardDates.LastDateStarts
            };
        }
    }
}