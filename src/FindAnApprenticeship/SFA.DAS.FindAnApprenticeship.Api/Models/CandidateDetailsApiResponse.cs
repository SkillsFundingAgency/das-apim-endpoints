using SFA.DAS.FindAnApprenticeship.Application.Queries.Candidate.GetCandidateDetails;
using System;

namespace SFA.DAS.FindAnApprenticeship.Api.Models;

public record CandidateDetailsApiResponse
{
    public Guid Id { get; set; }
    public string GovUkIdentifier { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public CandidateAddressApiResponse Address { get; set; }

    public static implicit operator CandidateDetailsApiResponse(GetCandidateDetailsQueryResult source)
    {
        if (source is null) return new CandidateDetailsApiResponse();

        return new CandidateDetailsApiResponse
        {
            GovUkIdentifier = source.GovUkIdentifier,
            Email = source.Email,
            FirstName = source.FirstName,
            LastName = source.LastName,
            MiddleName = source.MiddleName,
            PhoneNumber = source.PhoneNumber,
            Id = source.Id,
            Address = source.Address,
        };
    }
}

public record CandidateAddressApiResponse
{
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? AddressLine3 { get; set; }
    public string? AddressLine4 { get; set; }
    public string Postcode { get; set; }
    public string Uprn { get; set; }

    public static implicit operator CandidateAddressApiResponse(CandidateAddress source)
    {
        if (source is null) return null;

        return new CandidateAddressApiResponse
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