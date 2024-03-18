using System;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Candidate.GetCandidateDetails;

public record GetCandidateDetailsQueryResult
{
    public Guid Id { get; set; }
    public string GovUkIdentifier { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public CandidateAddress Address { get; set; }
}

public record CandidateAddress
{
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? AddressLine3 { get; set; }
    public string? AddressLine4 { get; set; }
    public string Postcode { get; set; }
    public string Uprn { get; set; }

    public static implicit operator CandidateAddress(GetAddressApiResponse source)
    {
        if (source is null) return null;

        return new CandidateAddress
        {
            AddressLine1 = source.AddressLine1,
            AddressLine2 = source.AddressLine2,
            AddressLine3 = source.AddressLine3,
            AddressLine4 = source.AddressLine4,
            Postcode = source.Postcode,
            Uprn = source.Uprn
        };
    }
}