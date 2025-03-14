using static SFA.DAS.ToolsSupport.InnerApi.Responses.GetApprenticeshipChangeOfProviderChainResponse;
using ApprenticeshipUpdate = SFA.DAS.ToolsSupport.InnerApi.Responses.ApprenticeshipUpdate;

namespace SFA.DAS.ToolsSupport.Application.Queries;

public class GetApprenticeshipQueryResult
{
    public long ApprenticeshipId { get; set; }
    public long EmployerAccountId { get; set; }
    public string PaymentStatus { get; set; }
    public string AgreementStatus { get; set; }
    public string? ConfirmationStatusDescription { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string Email { get; set; }
    public string Uln { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string CohortReference { get; set; }
    public string EmployerReference { get; set; }

    public string LegalEntity { get; set; }

    public string TrainingProvider { get; set; }
    public long UKPRN { get; set; }
    public string Trainingcourse { get; set; }
    public string ApprenticeshipCode { get; set; }

    public DateTime? TrainingStartDate { get; set; }
    public DateTime? TrainingEndDate { get; set; }
    public Decimal? TrainingCost { get; set; }

    public string? Version { get; set; }
    public string Option { get; set; }
    public DateTime? PauseDate { get; set; }
    public DateTime? StopDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public string PaymentStatusTagColour { get; set; }
    public bool? MadeRedundant { get; set; }

    public DateTime? OverlappingTrainingDateRequestCreatedOn { get; set; }
    public List<ChangeOfProviderLink> ChangeOfProviderChain { get; set; }
    public PendingChangesResponse PendingChanges { get; set; }
}

public class PendingChangesResponse
{
    public string Description { get; set; }
    public List<PendingChange> Changes { get; set; } = new ();
}

public class PendingChange
{
    public string Name { get; set; }
    public string OriginalValue { get; set; }
    public string NewValue { get; set; }
}