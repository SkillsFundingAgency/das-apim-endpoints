using MediatR;
using SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeshipRegistration;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeCommitments.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeCommitments
{
    public class GetApprenticeshipRegistrationByEmailQueryHandler : IRequestHandler<GetApprenticeshipRegistrationByEmailQuery, GetApprenticeshipRegistrationQueryResult>
    {
        private readonly IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> _apprenticeCommitmentsApiClient;

        public GetApprenticeshipRegistrationByEmailQueryHandler(IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> apprenticeCommitmentsApiClient)
        {
            _apprenticeCommitmentsApiClient = apprenticeCommitmentsApiClient;
        }
        public async Task<GetApprenticeshipRegistrationQueryResult> Handle(GetApprenticeshipRegistrationByEmailQuery request, CancellationToken cancellationToken)
        {
            var task = await _apprenticeCommitmentsApiClient.Get<GetApprenticeshipRegistrationQueryResult>(new GetApprenticeshipRegistrationByEmailRequest(request.Email));

            return task;
        }
    }
}
