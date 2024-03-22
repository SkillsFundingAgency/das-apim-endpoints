using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
public class PutCandidateAddressApiRequest : IPutApiRequest
{
    private readonly string _govUkIdentifier;
    public object Data { get; set; }

    public PutCandidateAddressApiRequest(string govIdentifier, PutCandidateAddressApiRequestData data)
    {
        _govUkIdentifier = govIdentifier;
        Data = data;
    }

    public string PutUrl => $"api/addresses/{_govUkIdentifier}";


}
public class PutCandidateAddressApiRequestData
{
    public string Email { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string AddressLine3 { get; set; }
    public string AddressLine4 { get; set; }
    public string Postcode { get; set; }
    public string Uprn { get; set; }
}