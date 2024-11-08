using System.Net;
using MediatR;
using SFA.DAS.EmployerPR.Common;
using SFA.DAS.EmployerPR.Infrastructure;
using SFA.DAS.EmployerPR.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.PensionRegulator;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.PensionsRegulator;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerPR.Application.Requests.Queries.ValidateRequest;

public class ValidatePermissionsRequestQueryHandler(
    IProviderRelationshipsApiRestClient _prApiClient,
    IAccountsApiClient<AccountsConfiguration> _accountsApiClient,
    IPensionRegulatorApiClient<PensionRegulatorApiConfiguration> _tprApiClient
    ) : IRequestHandler<ValidatePermissionsRequestQuery, ValidatePermissionsRequestQueryResult>
{
    public async Task<ValidatePermissionsRequestQueryResult> Handle(ValidatePermissionsRequestQuery query, CancellationToken cancellationToken)
    {
        ValidatePermissionsRequestQueryResult result = new();

        var permissionRequestResponse = await _prApiClient.GetRequest(query.RequestId, cancellationToken);

        if (!permissionRequestResponse.ResponseMessage.IsSuccessStatusCode)
        {
            result.IsRequestValid = false;
            return result;
        }

        GetRequestResponse permissionRequest = permissionRequestResponse.GetContent()!;

        if (permissionRequest.RequestType != RequestType.CreateAccount.ToString())
        {
            result.IsRequestValid = false;
            return result;
        }

        result.IsRequestValid = true;
        result.Status = permissionRequest.Status;

        var accountHistoriesResponseTask = _accountsApiClient.GetWithResponseCode<GetAccountHistoriesByPayeResponse>(new GetAccountHistoriesByPayeRequest(permissionRequest.EmployerPAYE));

        var tprResponseTask = _tprApiClient.GetWithResponseCode<IEnumerable<PensionRegulatorOrganisation>>(new GetPensionsRegulatorOrganisationsRequest(permissionRequest.EmployerAORN, permissionRequest.EmployerPAYE));

        await Task.WhenAll(accountHistoriesResponseTask, tprResponseTask);

        var accountHistoriesResponse = accountHistoriesResponseTask.Result;
        var tprResponse = tprResponseTask.Result;

        result.HasEmployerAccount = accountHistoriesResponse.StatusCode == HttpStatusCode.OK;

        result.HasValidPaye = tprResponse.StatusCode == HttpStatusCode.OK && tprResponse.Body.Any(o => o.Status == string.Empty);

        return result;
    }
}
