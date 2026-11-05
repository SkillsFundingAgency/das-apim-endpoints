using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Responses;

public class PostValidateSelectMultipleLearnersResponse
{
    public IEnumerable<LearnerDataValidationError> ValidationErrors { get; set; } = new List<LearnerDataValidationError>();
}

public class LearnerDataValidationError
{
    public string EmployerName { get; set; }
    public string Uln { get; set; }
    public string ApprenticeName { get; set; }
    public List<Error> Errors { get; set; }
}

public class Error
{
    public string Property { get; set; }
    public string ErrorText { get; set; }
}