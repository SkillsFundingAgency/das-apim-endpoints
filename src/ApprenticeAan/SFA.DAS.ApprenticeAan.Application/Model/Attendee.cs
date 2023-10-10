namespace SFA.DAS.ApprenticeAan.Application.Model;

public record struct Attendee(Guid MemberId, string UserType, string MemberName, DateTime? AddedDate);
