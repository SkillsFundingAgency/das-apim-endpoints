using MediatR;
using Newtonsoft.Json;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeAccounts
{
    public  class GetApprenticeAccountByNameQueryHandler : IRequestHandler<GetApprenticeAccountByNameQuery, GetApprenticeAccountByNameQueryResult>
    {
        private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _accountsApiClient;

        public GetApprenticeAccountByNameQueryHandler(IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> accountsApiClient)
        => _accountsApiClient = accountsApiClient;

        public async Task<GetApprenticeAccountByNameQueryResult> Handle(GetApprenticeAccountByNameQuery request, CancellationToken cancellationToken)
        {
            var result = await _accountsApiClient.Get<List<ApprenticeAccount>>(new GetApprenticeAccountByNameRequest(request.FirstName, request.LastName, request.DateOfBirth));

            var apprentice = result.Select(x => new Apprentice
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                ApprenticeId = x.Id,
                Email = x.Email.Address,
                DateOfBirth = x.DateOfBirth,
                TermsOfUseAccepted = x.TermsOfUseAccepted,
            }).ToList();

            return new GetApprenticeAccountByNameQueryResult { Apprentices = apprentice};
        }
    }
}
