using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

public record GetAddressApiResponse
{
    public string AddressLine1 { get; set; } = null!;
    public string? AddressLine2 { get; set; }
    public string? Town { get; set; }
    public string? County { get; set; }
    public string Postcode { get; set; }
    public Guid CandidateId { get; set; }
}