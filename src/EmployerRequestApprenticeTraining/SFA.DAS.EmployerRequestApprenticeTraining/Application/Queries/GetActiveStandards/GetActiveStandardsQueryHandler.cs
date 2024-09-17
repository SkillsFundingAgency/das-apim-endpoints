using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Standard = SFA.DAS.SharedOuterApi.Models.RequestApprenticeTraining.Standard;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetActiveStandards
{
    public class GetActiveStandardsQueryHandler : IRequestHandler<GetActiveStandardsQuery, GetActiveStandardsResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public GetActiveStandardsQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _coursesApiClient = coursesApiClient;
        }

        public async Task<GetActiveStandardsResult> Handle(GetActiveStandardsQuery request, CancellationToken cancellationToken)
        {
            var apiStandards = await _coursesApiClient.Get<GetStandardsListResponse>(new GetActiveStandardsListRequest());

            return new GetActiveStandardsResult
            {
                Standards = apiStandards.Standards.Select(s => (Standard)s).ToList()
            };

        }
    }
}
