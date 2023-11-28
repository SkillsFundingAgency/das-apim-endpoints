namespace SFA.DAS.AdminAan.Domain;

public record GetMemberResponse(Guid MemberId, string Email, string LastName, string? OrganisationName, int? RegionId, string UserType, bool? IsRegionalChair, string FullName, ApprenticeModel? Apprentice, EmployerModel? Employer);

public record ApprenticeModel(Guid ApprenticeId);

public record EmployerModel(long AccountId, Guid UserRef);
