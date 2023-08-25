namespace SFA.DAS.EmployerAan.Models;
public record struct Attendee(Guid MemberId, string UserType, string MemberName, DateTime? AddedDate);