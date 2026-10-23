using SFA.DAS.Apim.Shared.Interfaces;
using System.Collections.Generic;

namespace SFA.DAS.EmployerFinance.InnerApi.Requests.Commitments;

public sealed record GetCommittedLearnersCostByAccountIdRequest(
    long? AccountId = null,
    int PageNumber = 1,
    int PageItemCount = 1000,
    long? TransferSenderId = null) : IGetApiRequest
{
    public string GetUrl
    {
        get
        {
            var queryParams = new List<string>();

            if (AccountId.HasValue)
                queryParams.Add($"accountId={AccountId.Value}");

            if (TransferSenderId.HasValue)
                queryParams.Add($"transferSenderId={TransferSenderId.Value}");

            queryParams.Add($"pageNumber={PageNumber}");
            queryParams.Add($"pageItemCount={PageItemCount}");

            return $"api/apprenticeships?{string.Join("&", queryParams)}";
        }
    }
}