using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

public record GetAddressApiResponse
{
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? AddressLine3 { get; set; }
    public string? AddressLine4 { get; set; }
    public string Postcode { get; set; }
    public string Uprn { get; set; }
    public Guid CandidateId { get; set; }
}