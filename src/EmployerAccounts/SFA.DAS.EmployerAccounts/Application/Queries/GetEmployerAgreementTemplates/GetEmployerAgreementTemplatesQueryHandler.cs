using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetEmployerAgreementTemplates;
public class GetEmployerAgreementTemplatesQueryHandler(IAccountsApiClient<AccountsConfiguration> _accountsApiClient, ILogger<GetEmployerAgreementTemplatesQueryHandler> _logger) : IRequestHandler<GetEmployerAgreementTemplatesQuery, GetEmployerAgreementTemplatesResponse>
{
    public async Task<GetEmployerAgreementTemplatesResponse> Handle(GetEmployerAgreementTemplatesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting Employer Agreement Templates from employer accounts api");

        var response = await _accountsApiClient.Get<GetEmployerAgreementTemplatesResponse>(new GetEmployerAgreementTemplatesRequest());

        return response;
    }
}

