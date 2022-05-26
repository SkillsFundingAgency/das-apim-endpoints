using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Application.Queries.Routes
{
    public class GetRoutesQueryHandler : IRequestHandler<GetRoutesQuery, GetRoutesQueryResult>
    {
        private readonly ICourseService _courseService;

        public GetRoutesQueryHandler (ICourseService courseService)
        {
            _courseService = courseService;
        }
        public async Task<GetRoutesQueryResult> Handle(GetRoutesQuery request, CancellationToken cancellationToken)
        {
            var response = await _courseService.GetRoutes();
            
            return new GetRoutesQueryResult
            {
                Routes = response.Routes
            };
        }
    }
}