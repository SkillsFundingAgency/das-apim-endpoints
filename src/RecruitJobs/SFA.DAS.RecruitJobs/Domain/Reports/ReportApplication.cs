using System.Collections.Generic;
using System.Text.Json;
using SFA.DAS.SharedOuterApi.Types.Models;

namespace SFA.DAS.RecruitJobs.Domain.Reports;

public record ReportApplication
{
    public Guid Id { get; init; }
    public Guid CandidateId { get; init; }
    public ReportCandidate? Candidate { get; init; }
    public string? Support { get; init; }
    public ReportEmploymentLocation? EmploymentLocation { get; init; }
}

public record ReportCandidate
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public DateTime? DateOfBirth { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public ReportCandidateAddress? Address { get; init; }
}

public record ReportCandidateAddress
{
    public string? AddressLine1 { get; init; }
    public string? AddressLine2 { get; init; }
    public string? Town { get; init; }
    public string? County { get; init; }
    public string? Postcode { get; init; }
}

public record ReportEmploymentLocation
{
    public List<ReportEmploymentAddress>? Addresses { get; init; }
}

public record ReportEmploymentAddress
{
    public bool IsSelected { get; init; }
    public short AddressOrder { get; init; }
    public string? FullAddress { get; init; }

    public Address? GetAddress() => FullAddress is not null
        ? JsonSerializer.Deserialize<Address>(FullAddress)
        : null;
}
