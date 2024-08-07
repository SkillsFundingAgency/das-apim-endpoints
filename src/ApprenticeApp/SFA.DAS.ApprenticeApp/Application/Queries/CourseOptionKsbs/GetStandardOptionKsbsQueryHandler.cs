using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.Courses.Requests;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Services;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Queries.CourseOptionKsbs
{
    public class GetStandardOptionKsbsQueryHandler : IRequestHandler<GetStandardOptionKsbsQuery, GetStandardOptionKsbsQueryResult>
    {
        private readonly CourseApiClient _courseApiClient;
        public GetStandardOptionKsbsQueryHandler(CourseApiClient courseApiClient)
        {
            _courseApiClient = courseApiClient;
        }

        public async Task<GetStandardOptionKsbsQueryResult> Handle(GetStandardOptionKsbsQuery request, CancellationToken cancellationToken)
        {
            var ksbs = await _courseApiClient.Get<GetStandardOptionKsbsResult>(new GetStandardOptionKsbsRequest(request.Id, request.Option));
            return new GetStandardOptionKsbsQueryResult { Ksbs = ksbs };
        }
    }

    public class GetStandardOptionKsbsResult
    {
        public Ksb[] Ksbs { get; set; }
    }
}