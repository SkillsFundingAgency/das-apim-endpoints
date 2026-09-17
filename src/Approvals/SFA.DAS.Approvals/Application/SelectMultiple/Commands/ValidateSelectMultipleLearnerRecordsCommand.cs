using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.Approvals.Application.SelectMultiple.Commands;

public class ValidateSelectMultipleLearnerRecordsCommand : IRequest
{
    public long ProviderId { get; set; }
    public long? AccountLegalEntityId { get; set; }
    public IEnumerable<long> LearnerIds { get; set; }
}