using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
public class GetAccountHistoriesByPayeResponse
{
    public long AccountId { get; set; }
    public DateTime AddedDate { get; set; }
    public DateTime? RemovedDate { get; set; }
}
