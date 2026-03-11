using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.Domain.Recruit;

public class Qualification
{
    public string QualificationType { get; set; }
    public string Subject { get; set; }
    public string Grade { get; set; }
    public int? Level { get; set; }
    public QualificationWeighting? Weighting { get; set; }
    public string OtherQualificationName { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum QualificationWeighting
{
    Essential,
    Desired
}