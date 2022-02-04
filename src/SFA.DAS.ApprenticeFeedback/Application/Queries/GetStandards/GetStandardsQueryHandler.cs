using MediatR;
using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Application.Queries.GetStandards
{
    public class GetStandardsQueryHandler : IRequestHandler<GetStandardsQuery, GetStandardsResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _apiClient;

        public GetStandardsQueryHandler(ICoursesApiClient<CoursesApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetStandardsResult> Handle(GetStandardsQuery request, CancellationToken cancellationToken)
        {
            var coursesResponse = await _apiClient.Get<GetStandardsListResponse>(new GetStandardsExportRequest());

            return new GetStandardsResult
            {
                Standards = coursesResponse.Standards
            };
        }
    }
}
