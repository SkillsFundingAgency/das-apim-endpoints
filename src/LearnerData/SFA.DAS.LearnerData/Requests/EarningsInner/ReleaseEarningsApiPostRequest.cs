using System.Text.Json.Serialization;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.LearnerData.Enums;

namespace SFA.DAS.LearnerData.Requests.EarningsInner;

public class ReleaseEarningsApiPostRequest(Guid learningKey, ReleaseEarningsRequest data) : IPostApiRequest<ReleaseEarningsRequest>
{
    public string PostUrl { get; } = $"learning/{learningKey}/release-earnings";
    public ReleaseEarningsRequest Data { get; set; } = data;
}

public class ReleaseEarningsRequest
{
    public Guid LearnerKey { get; set; }
    public string LearnerRef { get; set; } = string.Empty;
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ReleaseType ReleaseType { get; set; }
    public List<Guid> EnglishAndMathsCourseKeys { get; set; } = [];
}
