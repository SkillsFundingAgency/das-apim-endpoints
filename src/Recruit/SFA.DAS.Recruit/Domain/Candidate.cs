#nullable enable
using System;

namespace SFA.DAS.Recruit.Domain;

public record Candidate
{
    public Guid Id { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Email { get; set; }
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleNames { get; set; }
    public string? PhoneNumber { get; set; }

    public CandidateAddress? Address { get; set; }
}