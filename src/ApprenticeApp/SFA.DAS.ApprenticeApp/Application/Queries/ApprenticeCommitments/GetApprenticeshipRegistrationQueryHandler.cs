using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeCommitments.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeshipRegistration
{
    public class GetApprenticeshipRegistrationQueryHandler : IRequestHandler<GetApprenticeshipRegistrationQuery, GetApprenticeshipRegistrationQueryResult>
    {
        private readonly IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> _apprenticeCommitmentsApiClient;

        public GetApprenticeshipRegistrationQueryHandler(IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> apprenticeCommitmentsApiClient)
        {
            _apprenticeCommitmentsApiClient = apprenticeCommitmentsApiClient;
        }
        public async Task<GetApprenticeshipRegistrationQueryResult> Handle(GetApprenticeshipRegistrationQuery request, CancellationToken cancellationToken)
        {
            var task = await _apprenticeCommitmentsApiClient.Get<GetApprenticeshipRegistrationQueryResult>(new GetApprenticeshipRegistrationRequest(request.ApprenticeshipId));

            return task;
        }
    }
}
