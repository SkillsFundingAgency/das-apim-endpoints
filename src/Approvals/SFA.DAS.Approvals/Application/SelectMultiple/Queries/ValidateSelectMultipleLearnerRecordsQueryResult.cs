using System.Collections.Generic;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Application.SelectMultiple.Queries;

public class ValidateSelectMultipleLearnerRecordsQueryResult
{
    public IEnumerable<LearnerDataValidationError> ValidationErrors { get; set; } = new List<LearnerDataValidationError>();
}