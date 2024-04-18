using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CoursesApi.Responses;

public class GetFrameworksApiResponse
{
    [JsonPropertyName("frameworks")]
    public List<GetFrameworkApiResponseItem> Frameworks { get; set; }
}

public class GetFrameworkApiResponseItem
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    public int Level { get; set; }
}