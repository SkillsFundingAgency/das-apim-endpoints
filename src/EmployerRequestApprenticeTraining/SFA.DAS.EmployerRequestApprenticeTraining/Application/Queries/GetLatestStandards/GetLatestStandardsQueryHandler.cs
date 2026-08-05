using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using Standard = SFA.DAS.SharedOuterApi.Types.Models.RequestApprenticeTraining.Standard;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLatestStandards
{
    public class GetLatestStandardsQueryHandler : IRequestHandler<GetLatestStandardsQuery, GetLatestStandardsResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public GetLatestStandardsQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _coursesApiClient = coursesApiClient;
        }

        public async Task<GetLatestStandardsResult> Handle(GetLatestStandardsQuery request, CancellationToken cancellationToken)
        {
            var apiStandards = await _coursesApiClient.Get<GetStandardsListResponse>(new GetAvailableToStartStandardsListRequest());

            return new GetLatestStandardsResult
            {
                Standards = apiStandards.Standards.Select(s => (Standard)s).ToList()
            };

        }
    }
}
