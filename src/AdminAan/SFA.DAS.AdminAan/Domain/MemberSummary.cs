namespace SFA.DAS.AdminAan.Domain;

public record MemberSummary(Guid MemberId, string FullName, int? RegionId, string? RegionName, MemberUserType UserType, bool IsRegionalChair, DateTime JoinedDate);
