using System.Collections.Generic;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Application.SelectMultiple.Commands;

public class SelectMultipleAddDraftApprenticeshipsCommand : IRequest<SelectMultipleAddDraftApprenticeshipsResult>
{
    public long ProviderId { get; set; }
    public long? AccountLegalEntityId { get; set; }
    public string AgreementId { get; set; }
    public IEnumerable<long> LearnerIds { get; set; }
    public long AccountId { get; set; }
    public UserInfo UserInfo { get; set; }
}