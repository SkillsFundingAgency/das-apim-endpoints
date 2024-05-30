using Newtonsoft.Json;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;


public class GetPreferencesApiResponse
{
    [JsonProperty("preferences")]
    public List<Preference> Preferences { get; set; }
}

public class Preference
{
    [JsonProperty("preferenceId")]
    public Guid PreferenceId { get; set; }

    [JsonProperty("preferenceMeaning")]
    public string PreferenceMeaning { get; set; }

    [JsonProperty("preferenceHint")]
    public string PreferenceHint { get; set; }
}