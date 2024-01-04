namespace SFA.DAS.ApprenticeAan.Application.Models;

public record struct Attendee(Guid MemberId, string UserType, string MemberName, DateTime? AddedDate);
