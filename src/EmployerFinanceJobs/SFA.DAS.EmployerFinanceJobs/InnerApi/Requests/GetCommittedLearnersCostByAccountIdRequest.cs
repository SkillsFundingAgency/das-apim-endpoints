using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.EmployerFinanceJobs.Domain.Enums;

namespace SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;

public sealed record GetCommittedLearnersCostByAccountIdRequest(
    long? AccountId = null,
    int PageNumber = 1,
    int PageItemCount = 1000,
    long? TransferSenderId = null,
    List<ApprenticeshipStatus>? Statuses = null,
    long? ProviderId = null,
    string? SortField = null,
    bool? ReverseSort = false,
    DateTime? LastUpdated = null) : IGetApiRequest
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

            if (Statuses != null && Statuses.Any())
            {
                queryParams.AddRange(Statuses.Select(status => $"statuses={status}"));
            }

            if (ProviderId.HasValue)
                queryParams.Add($"providerId={ProviderId.Value}");

            if (!string.IsNullOrEmpty(SortField))
                queryParams.Add($"sortField={SortField}");

            if (ReverseSort == true)
                queryParams.Add($"reverseSort={ReverseSort.Value}");

            if(LastUpdated.HasValue)
            {
                queryParams.Add($"lastUpdated={LastUpdated.Value:yyyy-MM-ddTHH:mm:ss}");
            }

            queryParams.Add($"pageNumber={PageNumber}");
            queryParams.Add($"pageItemCount={PageItemCount}");
            

            return $"api/apprenticeships?{string.Join("&", queryParams)}";
        }
    }
}
