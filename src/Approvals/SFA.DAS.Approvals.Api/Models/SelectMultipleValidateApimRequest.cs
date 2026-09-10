using System;
using System.Collections.Generic;

namespace SFA.DAS.Approvals.Api.Models;

public class SelectMultipleValidateApimRequest
{
    public long ProviderId { get; set; }
    public IEnumerable<LearnerSummary> Learners { get; set; }
    public long? AccountLegalEntityId { get; set; }
}

public class LearnerSummary
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Name => $"{FirstName} {LastName}";
    public long Uln { get; set; }
    public string CourseName { get; set; }
    public DateTime StartDate { get; set; }    
}