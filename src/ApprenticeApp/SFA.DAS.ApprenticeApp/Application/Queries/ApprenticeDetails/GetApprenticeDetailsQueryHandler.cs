using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.ApprenticeApp.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Homepage
{
    public class GetApprenticeDetailsQueryHandler : IRequestHandler<GetApprenticeDetailsQuery, GetApprenticeDetailsQueryResult>
    {
        private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _accountsApiClient;
        private readonly IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> _commitmentsApiClient;

        public GetApprenticeDetailsQueryHandler(
            IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> accountsApiClient,
            IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> commitmentsApiClient
            )
        {
            _accountsApiClient = accountsApiClient;
            _commitmentsApiClient = commitmentsApiClient;
        }

        public async Task<GetApprenticeDetailsQueryResult> Handle(GetApprenticeDetailsQuery request, CancellationToken cancellationToken)
        {
            var apprenticeTask = _accountsApiClient.Get<Apprentice>(new GetApprenticeRequest(request.ApprenticeId));
            var myApprenticeshipTask = _accountsApiClient.Get<MyApprenticeship>(new GetMyApprenticeshipRequest(request.ApprenticeId));

            await Task.WhenAll(apprenticeTask, myApprenticeshipTask);

            return new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = await apprenticeTask,
                    MyApprenticeship = await myApprenticeshipTask
                }
            };
        }
    }
}
