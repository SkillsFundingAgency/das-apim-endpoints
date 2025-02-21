using System.ComponentModel;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;

namespace SFA.DAS.ToolsSupport.InnerApi.Responses;

public class GetApprovedApprenticeshipsByCohortIdResponse
{
    public List<SupportApprenticeshipDetails> ApprovedApprenticeships { get; set; }
}

// TODO remove unused fields
public class SupportApprenticeshipDetails
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Uln { get; set; }
    public string EmployerName { get; set; }
    public string ProviderName { get; set; }
    public long ProviderId { get; set; }
    public string CourseName { get; set; }
    public GetApprenticeshipUpdatesResponse.DeliveryModel DeliveryModel { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime PauseDate { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public ApprenticeshipStatus ApprenticeshipStatus { get; set; }
    public IEnumerable<Alerts> Alerts { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string ProviderRef { get; set; }
    public string EmployerRef { get; set; }
    public decimal? TotalAgreedPrice { get; set; }
    public string CohortReference { get; set; }
    public long AccountLegalEntityId { get; set; }
    public ConfirmationStatus? ConfirmationStatus { get; set; }
    public long? TransferSenderId { get; set; }
    public bool HasHadDataLockSuccess { get; set; }
    public string CourseCode { get; set; }
    public decimal? Cost { get; set; }
    public AgreementStatus AgreementStatus { get; set; }
    public DateTime? StopDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public bool? MadeRedundant { get; set; }
    public string StandardUId { get; set; }
    public string TrainingCourseVersion { get; set; }
    public bool TrainingCourseVersionConfirmed { get; set; }
    public string TrainingCourseOption { get; set; }
    public long EmployerAccountId { get; internal set; }
    public int? EmploymentPrice { get; set; }
    public DateTime? EmploymentEndDate { get; set; }
}

public enum PaymentStatus : short
{
    //PendingApproval = 0, //TODO : Remove later
    Active = 1,
    Paused = 2,
    Withdrawn = 3,
    Completed = 4
}

public enum ApprenticeshipStatus : short
{
    [Description("Waiting to start")]
    WaitingToStart = 0,
    [Description("Live")]
    Live = 1,
    [Description("Paused")]
    Paused = 2,
    [Description("Stopped")]
    Stopped = 3,
    [Description("Completed")]
    Completed = 4,
    [Description("Unknown")]
    Unknown
}

[Flags]
public enum Alerts
{
    [Description("ILR data mismatch")]
    IlrDataMismatch = 0,

    [Description("Changes pending")]
    ChangesPending = 1,

    [Description("Changes requested")]
    ChangesRequested = 2,

    [Description("Changes for review")]
    ChangesForReview = 3,

    [Description("Confirm dates")]
    ConfirmDates = 4
}

public enum ConfirmationStatus : short
{
    [Description("N/A")]
    NA = 4,
    Overdue = 3,
    Unconfirmed = 2,
    Confirmed = 1
}

public enum AgreementStatus : short
{
    [Description("Not agreed")]
    NotAgreed = 0,
    [Description("Employer agreed")]
    EmployerAgreed = 1,
    [Description("Provider agreed")]
    ProviderAgreed = 2,
    [Description("Both agreed")]
    BothAgreed = 3
}