using MediatR;
using SFA.DAS.EmployerPR.Application.Requests.Queries.GetRequest;
using SFA.DAS.EmployerPR.Common;
using SFA.DAS.EmployerPR.Infrastructure;

namespace SFA.DAS.EmployerPR.Application.Requests.Queries.ValidateRequest;

public class ValidatePermissionsRequestQueryHandler(
    IProviderRelationshipsApiRestClient _apiClient
    ) : IRequestHandler<ValidatePermissionsRequestQuery, ValidatePermissionsRequestQueryResult>
{
    public async Task<ValidatePermissionsRequestQueryResult> Handle(ValidatePermissionsRequestQuery query, CancellationToken cancellationToken)
    {
        ValidatePermissionsRequestQueryResult result = new();

        var response = await _apiClient.GetRequest(query.RequestId, cancellationToken);

        if (!response.ResponseMessage.IsSuccessStatusCode)
        {
            result.IsRequestValid = false;
            return result;
        }

        GetRequestQueryResult permissionRequest = response.GetContent()!;

        if (permissionRequest.RequestType != RequestType.CreateAccount)
        {
            result.IsRequestValid = false;
            return result;
        }

        result.IsRequestValid = true;
        result.Status = permissionRequest.Status;
        return result;
    }
}
