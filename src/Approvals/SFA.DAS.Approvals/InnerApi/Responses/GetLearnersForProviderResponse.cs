using System;
using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Responses;

public record GetLearnersForProviderResponse
{
    public DateTime? LastSubmissionDate { get; set; }
    public IEnumerable<LearnerDataRecord> Data { get; set; } = new List<LearnerDataRecord>();
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int Page { get; set; } = 1;
}

public record LearnerDataRecord
{
    public long Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public long Uln { get; set; }
    public long Ukprn { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime Dob { get; set; }
    public int AcademicYear { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime PlannedEndDate { get; set; }
    public int? PercentageLearningToBeDelivered { get; set; }
    public int EpaoPrice { get; set; }
    public int TrainingPrice { get; set; }
    public string AgreementId { get; set; }
    public int StandardCode { get; set; }
    public bool IsFlexiJob { get; set; }
    public int PlannedOTJTrainingHours { get; set; }
    public DateTime ReceivedDate { get; set; }
    public string CorrelationId { get; set; }
    public string ConsumerReference { get; set; }
}