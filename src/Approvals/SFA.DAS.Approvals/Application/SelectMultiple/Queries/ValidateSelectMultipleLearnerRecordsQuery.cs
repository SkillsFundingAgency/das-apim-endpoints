using System.Collections.Generic;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Application.SelectMultiple.Queries;

public class ValidateSelectMultipleLearnerRecordsQuery : IRequest<ValidateSelectMultipleLearnerRecordsQueryResult>
{
    public long ProviderId { get; set; }
    public long? AccountLegalEntityId { get; set; }
    public string AgreementId { get; set; }
    public IEnumerable<long> LearnerIds { get; set; }
    public UserInfo UserInfo { get; set; }
}
