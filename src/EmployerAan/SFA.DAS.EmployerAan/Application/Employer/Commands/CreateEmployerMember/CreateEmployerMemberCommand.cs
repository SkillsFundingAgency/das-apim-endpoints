﻿using MediatR;

namespace SFA.DAS.EmployerAan.Application.Employer.Commands.CreateEmployerMember;

public class CreateEmployerMemberCommand : IRequest<CreateEmployerMemberCommandResult>
{
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime JoinedDate { get; set; }
    public int? RegionId { get; set; }
    public string? OrganisationName { get; set; } = null!;
    public List<ProfileValue> ProfileValues { get; set; } = new();
    public List<MemberNotificationEventFormatValues>? MemberNotificationEventFormatValues { get; set; } = new();
    public List<MemberNotificationLocationValues>? MemberNotificationLocationValues { get; set; } = new();
    public bool ReceiveNotifications { get; set; }
    public long AccountId { get; init; }
    public Guid UserRef { get; init; }
}
public record class ProfileValue(int Id, string Value);
public record class MemberNotificationEventFormatValues(string EventFormat, int Ordering, bool ReceiveNotifications);
public record class MemberNotificationLocationValues(string Name, int Radius, double Latitude, double Longitude);