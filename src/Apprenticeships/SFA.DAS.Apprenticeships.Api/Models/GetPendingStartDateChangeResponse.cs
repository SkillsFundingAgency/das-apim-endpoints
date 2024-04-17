using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;

namespace SFA.DAS.Apprenticeships.Api.Models;

public class GetPendingStartDateChangeResponse
{
    public bool HasPendingStartDateChange { get; set; }
    public PendingStartDateChange? PendingStartDateChange { get; set; }
    public string ProviderName { get; set; }
    public string EmployerName { get; set; }

    public GetPendingStartDateChangeResponse(GetPendingStartDateChangeApiResponse apiResponse, string providerName, Guid apprenticeshipKey, string employerName)
    {
        HasPendingStartDateChange = apiResponse.HasPendingStartDateChange;
        PendingStartDateChange = apiResponse.PendingStartDateChange != null ? new PendingStartDateChange
        {
            Reason = apiResponse.PendingStartDateChange.Reason!,
            Ukprn = apiResponse.PendingStartDateChange.Ukprn,
            AccountLegalEntityId = apiResponse.PendingStartDateChange.AccountLegalEntityId,
            ApprenticeshipKey = apprenticeshipKey,
            ProviderApprovedDate = apiResponse.PendingStartDateChange.ProviderApprovedDate,
            EmployerApprovedDate = apiResponse.PendingStartDateChange.EmployerApprovedDate,
            Initiator = apiResponse.PendingStartDateChange.Initiator!,
            OriginalActualStartDate = apiResponse.PendingStartDateChange.OriginalActualStartDate,
            PendingActualStartDate = apiResponse.PendingStartDateChange.PendingActualStartDate
        } : null;
        ProviderName = providerName;
        EmployerName = employerName;
    }
}