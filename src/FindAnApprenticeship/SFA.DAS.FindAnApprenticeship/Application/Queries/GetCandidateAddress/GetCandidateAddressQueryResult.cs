using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidateAddress;
public class GetCandidateAddressQueryResult
{
    public bool IsAddressFromLookup { get; set; }
    public string? Postcode { get; set; }
    public string AddressLine1 { get; set; } = null!;
    public string? AddressLine2 { get; set; }
    public string Town { get; set; } = null!;
    public string? County { get; set; }

    public static implicit operator GetCandidateAddressQueryResult(GetCandidateAddressApiResponse source)
    {
        if (source == null)
        {
            return new GetCandidateAddressQueryResult()
            {
                IsAddressFromLookup = false,
                Postcode = null,
                AddressLine1 = null,
                AddressLine2 = null,
                Town = null,
                County = null
            };
        }
        return new GetCandidateAddressQueryResult
        {
            IsAddressFromLookup = !string.IsNullOrWhiteSpace(source.Uprn),
            Postcode = source.Postcode,
            AddressLine1 = source.AddressLine1,
            AddressLine2 = source.AddressLine2,
            Town = source.Town,
            County = source.County
        };
    }


}
