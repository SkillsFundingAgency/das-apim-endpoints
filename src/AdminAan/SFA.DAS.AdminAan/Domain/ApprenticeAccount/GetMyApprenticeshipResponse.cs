namespace SFA.DAS.AdminAan.Domain.ApprenticeAccount;

public record GetMyApprenticeshipResponse(long? Uln, long? ApprenticeshipId, string? EmployerName, DateTime? StartDate, DateTime? EndDate, long? TrainingProviderId, string? TrainingProviderName, string? TrainingCode, string? StandardUId);
