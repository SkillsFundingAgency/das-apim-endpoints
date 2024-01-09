namespace SFA.DAS.AdminAan.Domain.Courses;

public record GetStandardResponse(string Title, int Level, string Route, StandardVersionDetail VersionDetail);

public record StandardVersionDetail(DateTime? EarliestStartDate, DateTime? LatestStartDate, DateTime? LatestEndDate, DateTime? ApprovedForDelivery, int ProposedTypicalDuration, int ProposedMaxFunding);
