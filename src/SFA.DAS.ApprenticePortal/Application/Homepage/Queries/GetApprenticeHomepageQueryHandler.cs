using MediatR;
using SFA.DAS.ApprenticePortal.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.ApprenticePortal.InnerApi.ApprenticeCommitments.Requests;
using SFA.DAS.ApprenticePortal.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticePortal.Application.Homepage.Queries
{
    public class GetApprenticeHomepageQueryHandler : IRequestHandler<GetApprenticeHomepageQuery, GetApprenticeHomepageQueryResult>
    {
        private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _accountsApiClient;
        private readonly IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> _commitmentsApiClient;

        public GetApprenticeHomepageQueryHandler(
            IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> accountsApiClient,
            IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> commitmentsApiClient)
            => (_accountsApiClient, _commitmentsApiClient) = (accountsApiClient, commitmentsApiClient);

        public async Task<GetApprenticeHomepageQueryResult> Handle(GetApprenticeHomepageQuery request, CancellationToken cancellationToken)
        {
            var apprentice = await _accountsApiClient.Get<Apprentice>(new GetApprenticeRequest(request.ApprenticeId));
            var apprenticeship = await _commitmentsApiClient.Get<GetApprenticeApprenticeshipsResult>(new GetApprenticeApprenticeshipsRequest(request.ApprenticeId));

            return new GetApprenticeHomepageQueryResult
            {
                apprenticeHomepage = new ApprenticeHomepage
                {
                    ApprenticeId = request.ApprenticeId,
                    FirstName = apprentice?.FirstName,
                    LastName = apprentice?.LastName,
                    CourseName =    apprenticeship?.apprenticeships[0].CourseName,
                    EmployerName = apprenticeship?.apprenticeships[0].EmployerName,
                    ApprenticeshipComplete = apprenticeship?.apprenticeships[0].ConfirmedOn.HasValue
                }
            };
        }
    }
}
