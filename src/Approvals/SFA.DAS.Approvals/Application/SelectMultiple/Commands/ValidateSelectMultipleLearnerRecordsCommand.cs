using System.Collections.Generic;
using MediatR;
using SFA.DAS.Approvals.Application.Learners.Queries;

namespace SFA.DAS.Approvals.Application.SelectMultiple.Commands;

public class ValidateSelectMultipleLearnerRecordsCommand : IRequest
{
    public long ProviderId { get; set; }
    public long? AccountLegalEntityId { get; set; }
    public List<LearnerSummary> Learners { get; set; }
}
