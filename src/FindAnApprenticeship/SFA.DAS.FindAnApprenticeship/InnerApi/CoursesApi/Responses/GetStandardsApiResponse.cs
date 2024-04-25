using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CoursesApi.Responses;

public class GetStandardsApiResponse
{
    [JsonPropertyName("standards")]
    public List<GetStandardApiResponseItem> Standards { get; set; }
}

public class GetStandardApiResponseItem
{
    [JsonPropertyName("standardUId")]
    public string StandardUId { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    public int Level { get; set; }
}