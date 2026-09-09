using System;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Application.Apprentices.Commands.ChangePayments;

public class ChangePaymentsCommand : IRequest<Unit>
{
    public long AccountId { get; set; }
    public long ApprenticeshipId { get; set; }
    public DateTime? PaymentFreezeDate { get; set; }
    public UserInfo UserInfo { get; set; }
    public int? FreezePaymentsReason { get; set; }
}
