using System.Net;
using MediatR;
using SFA.DAS.EmployerPR.Application.Requests.Queries.GetRequest;
using SFA.DAS.EmployerPR.Common;
using SFA.DAS.EmployerPR.Infrastructure;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerPR.Application.Requests.Queries.ValidateRequest;

public class ValidatePermissionsRequestQueryHandler(
    IProviderRelationshipsApiRestClient _prApiClient,
    IAccountsApiClient<AccountsConfiguration> _accountsApiClient
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

        GetRequestQueryResult permissionRequest = permissionRequestResponse.GetContent()!;

        if (permissionRequest.RequestType != RequestType.CreateAccount)
        {
            result.IsRequestValid = false;
            return result;
        }

        result.IsRequestValid = true;
        result.Status = permissionRequest.Status;

        var accountHistoriesResponse = await _accountsApiClient.GetWithResponseCode<GetAccountHistoriesByPayeResponse>(new GetAccountHistoriesByPayeRequest(permissionRequest.EmployerPAYE));

        result.HasEmployerAccount = accountHistoriesResponse.StatusCode == HttpStatusCode.OK;

        return result;
    }
}
