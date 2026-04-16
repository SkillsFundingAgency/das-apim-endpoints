using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests;

public class GetLocationsByPostBulkPostcodeRequest(List<string> data) : IPostApiRequest
{
    public string PostUrl => "api/Postcodes/bulk";
    public object Data { get; set; } = data;
    public string Version => "2.0";
}