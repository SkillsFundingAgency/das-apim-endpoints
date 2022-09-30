using MediatR;
using SFA.DAS.Campaign.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Campaign.Application.Queries.Standard
{
    public class GetStandardQueryHandler : IRequestHandler<GetStandardQuery, GetStandardQueryResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public GetStandardQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _coursesApiClient = coursesApiClient;
        }

        public async Task<GetStandardQueryResult> Handle(GetStandardQuery request, CancellationToken cancellationToken)
        {
            var standard =
                await _coursesApiClient.Get<GetStandardListResponse>(new GetStandardRequest(request.StandardUId));

            return new GetStandardQueryResult
            {
                Standard = new Standard
                {
                    Title = standard.Title,
                    Level = standard.Level,
                    StandardUId = standard.StandardUId,
                    LarsCode = standard.LarsCode,
                    TimeToComplete = standard.ApprenticeshipFunding.Last().Duration,
                    MaxFunding = standard.ApprenticeshipFunding.Last().MaxEmployerLevyCap
                }
            };
        }
    }
}
