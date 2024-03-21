using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests;

public class GetLocationsByPostBulkPostcodeRequest : IPostApiRequest
{
    public GetLocationsByPostBulkPostcodeRequest(List<string> data)
    {
        Data = data;
    }

    public string PostUrl => "api/Postcodes/bulk";
    public object Data { get; set; }
}