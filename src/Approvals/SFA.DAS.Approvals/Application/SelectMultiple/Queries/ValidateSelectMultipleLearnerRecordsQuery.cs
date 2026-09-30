using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.Approvals.Application.SelectMultiple.Queries;

public class ValidateSelectMultipleLearnerRecordsQuery : IRequest<ValidateSelectMultipleLearnerRecordsQueryResult>
{
    public long ProviderId { get; set; }
    public long? AccountLegalEntityId { get; set; }
    public IEnumerable<long> LearnerIds { get; set; }
}