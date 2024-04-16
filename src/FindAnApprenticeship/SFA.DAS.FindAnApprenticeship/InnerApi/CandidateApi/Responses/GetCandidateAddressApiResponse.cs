using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
public class GetCandidateAddressApiResponse
{
    public Guid Id { get; set; }
    public string? Uprn { get; set; }
    public string AddressLine1 { get; set; } = null!;
    public string? AddressLine2 { get; set; }
    public string Town { get; set; } = null!;
    public string? County { get; set; }
    public string? Postcode { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public Guid CandidateId { get; set; }
}
