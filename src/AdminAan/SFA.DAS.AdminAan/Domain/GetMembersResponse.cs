namespace SFA.DAS.AdminAan.Domain;

public record GetMembersResponse(int Page, int PageSize, int TotalPages, int TotalCount, IEnumerable<MemberSummary> Members);
