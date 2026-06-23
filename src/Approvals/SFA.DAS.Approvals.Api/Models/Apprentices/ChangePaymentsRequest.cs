using System;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Api.Models.Apprentices;

public class ChangePaymentsRequest
{
    public DateTime? PaymentFreezeDate { get; set; }
    public UserInfo UserInfo { get; set; }
    public int? FreezePaymentsReason { get; set; }
}
