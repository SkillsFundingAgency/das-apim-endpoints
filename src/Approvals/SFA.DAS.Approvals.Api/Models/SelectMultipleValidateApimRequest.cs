using System.Collections.Generic;

namespace SFA.DAS.Approvals.Api.Models;

public class SelectMultipleValidateApimRequest
{
    public long ProviderId { get; set; }
    public IEnumerable<long> LearnerIds { get; set; }
    public long? AccountLegalEntityId { get; set; }
}