using MediatR;
using SFA.DAS.Assessors.InnerApi.Requests;
using SFA.DAS.Assessors.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Assessors.Application.Queries.GetStandardOptions
{
    public class GetStandardOptionsQueryHandler : IRequestHandler<GetStandardOptionsQuery, GetStandardOptionsResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public GetStandardOptionsQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _coursesApiClient = coursesApiClient;
        }

        public async Task<GetStandardOptionsResult> Handle(GetStandardOptionsQuery request, CancellationToken cancellationToken)
        {
            var standardOptionsListResponse = await _coursesApiClient.Get<GetStandardOptionsListResponse>(new GetStandardOptionsListRequest());

            return new GetStandardOptionsResult
            {
                Standards = standardOptionsListResponse.Standards
            };
        }
    }
}
