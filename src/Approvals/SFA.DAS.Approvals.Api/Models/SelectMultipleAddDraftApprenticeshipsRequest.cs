using System.Collections.Generic;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Api.Models;

public class SelectMultipleAddDraftApprenticeshipsRequest
{
    public long ProviderId { get; set; }
    public IEnumerable<long> LearnerIds { get; set; }
    public long? AccountLegalEntityId { get; set; }
    public string AgreementId { get; set; }
    public long AccountId { get; set; }
    public UserInfo UserInfo { get; set; }
    public long? CohortId { get; set; }
    public long? TransferSenderAccountId { get; set; }
    public string CohortReference { get; set; }
    public string EncodedPledgeApplicationId { get; set; }
}