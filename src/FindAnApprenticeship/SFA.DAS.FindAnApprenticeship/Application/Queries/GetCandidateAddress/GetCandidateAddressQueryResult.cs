using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidateAddress;
public class GetCandidateAddressQueryResult
{
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
                Postcode = null,
                AddressLine1 = null,
                AddressLine2 = null,
                Town = null,
                County = null
            };
        }
        return new GetCandidateAddressQueryResult
        {
            Postcode = source.Postcode ?? null,
            AddressLine1 = source.AddressLine1 ?? null,
            AddressLine2 = source.AddressLine2 ?? null,
            Town = source.Town ?? null,
            County = source.County ?? null
        };
    }


}
