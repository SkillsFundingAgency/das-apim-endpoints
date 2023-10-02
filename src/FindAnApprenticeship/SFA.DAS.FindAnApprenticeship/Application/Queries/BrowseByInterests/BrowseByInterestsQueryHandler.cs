using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.BrowseByInterests
{
    public class BrowseByInterestsQueryHandler : IRequestHandler<BrowseByInterestsQuery, BrowseByInterestsResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _apiClient;

        public BrowseByInterestsQueryHandler(
            ICoursesApiClient<CoursesApiConfiguration> apiClient)

        {
            _apiClient = apiClient;
        }

        public async Task<BrowseByInterestsResult> Handle(BrowseByInterestsQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.Get<GetRoutesResponse>(new GetRoutesRequest());

            return new BrowseByInterestsResult
            {
                Routes = result.Routes
            };
        }

    }
}