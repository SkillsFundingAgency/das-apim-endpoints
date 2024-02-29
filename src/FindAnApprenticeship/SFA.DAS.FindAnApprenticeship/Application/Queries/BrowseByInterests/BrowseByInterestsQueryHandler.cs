using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.BrowseByInterests
{
    public class BrowseByInterestsQueryHandler : IRequestHandler<BrowseByInterestsQuery, BrowseByInterestsResult>
    {
        private readonly ICourseService _courseService;

        public BrowseByInterestsQueryHandler(
            ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<BrowseByInterestsResult> Handle(BrowseByInterestsQuery request, CancellationToken cancellationToken)
        {
            var result = await _courseService.GetRoutes();

            return new BrowseByInterestsResult
            {
                Routes = result.Routes.ToList()
            };
        }

    }
}