namespace SFA.DAS.ApprenticeAan.Application.Entities;

public record struct Attendee(Guid MemberId, string UserType, string MemberName);
