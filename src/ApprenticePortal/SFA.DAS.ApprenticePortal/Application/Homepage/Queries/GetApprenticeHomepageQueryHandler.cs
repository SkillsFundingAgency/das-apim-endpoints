using MediatR;
using SFA.DAS.ApprenticePortal.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.ApprenticePortal.InnerApi.ApprenticeCommitments.Requests;
using SFA.DAS.ApprenticePortal.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Linq;
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
            var apprentice = _accountsApiClient.Get<Apprentice>(new GetApprenticeRequest(request.ApprenticeId));
            var apprenticeships = _commitmentsApiClient.Get<GetApprenticeApprenticeshipsResult>(new GetApprenticeApprenticeshipsRequest(request.ApprenticeId));

            await Task.WhenAll(apprentice, apprenticeships);

            return new GetApprenticeHomepageQueryResult
            {
                ApprenticeHomepage = new ApprenticeHomepage
                {                    
                    Apprentice = apprentice.Result,
                    Apprenticeship = apprenticeships.Result?.Apprenticeships.FirstOrDefault()
                },                
            };
        }
    }
}
