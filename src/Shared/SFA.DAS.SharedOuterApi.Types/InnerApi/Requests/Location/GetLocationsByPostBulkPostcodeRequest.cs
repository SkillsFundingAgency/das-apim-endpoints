using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Location;

public class GetLocationsByPostBulkPostcodeRequest(List<string> data) : IPostApiRequest
{
    public string PostUrl => "api/Postcodes/bulk";
    public object Data { get; set; } = data;
    public string Version => "2.0";
}