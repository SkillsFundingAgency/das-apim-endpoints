using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCourseDemandsOlderThan3Years
{
    public class GetCourseDemandsOlderThan3YearsQueryHandler : IRequestHandler<GetCourseDemandsOlderThan3YearsQuery, GetCourseDemandsOlderThan3YearsResult> 
    {
        private readonly IEmployerDemandApiClient<EmployerDemandApiConfiguration> _demandApiClient;

        public GetCourseDemandsOlderThan3YearsQueryHandler(IEmployerDemandApiClient<EmployerDemandApiConfiguration> demandApiClient)
        {
            _demandApiClient = demandApiClient;
        }

        public async Task<GetCourseDemandsOlderThan3YearsResult> Handle(GetCourseDemandsOlderThan3YearsQuery request, CancellationToken cancellationToken)
        {
            var apiResponse =
                await _demandApiClient.Get<GetEmployerDemandsOlderThan3YearsResponse>(
                    new GetEmployerDemandsOlderThan3YearsRequest());

            return new GetCourseDemandsOlderThan3YearsResult
            {
                EmployerDemandIds = apiResponse.EmployerDemandIds
            };
        }
    }
}