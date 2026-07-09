using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Roatp;

public class GetUkrlpProvidersRequest : IGetApiRequest
{
    public IEnumerable<int> Ukprns { get; set; } = [];
    public DateTime? UpdatedSinceDate { get; set; }

    public string GetUrl => $"/ukrlp/providers?{GenerateQueryParams()}";

    private string GenerateQueryParams()
    {
        var queryParamsList = new List<string>();
        queryParamsList.AddRange(Ukprns.Select(ukprn => $"ukprns={ukprn}"));
        if (UpdatedSinceDate.HasValue)
        {
            queryParamsList.Add($"updatedSince={UpdatedSinceDate.Value.ToString("yyyy-MM-dd")}");
        }
        var queryParams = string.Join("&", queryParamsList);
        return queryParams;
    }
}
