using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
public class PutCandidateAddressApiRequest : IPutApiRequest
{
    private readonly string _candidateId;
    public object Data { get; set; }

    public PutCandidateAddressApiRequest(string candidateId, PutCandidateAddressApiRequestData data)
    {
        _candidateId = candidateId;
        Data = data;
    }

    public string PutUrl => $"api/candidates/{_candidateId}/addresses";


}
public class PutCandidateAddressApiRequestData
{
    public string Email { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string AddressLine3 { get; set; }
    public string AddressLine4 { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Postcode { get; set; }
}