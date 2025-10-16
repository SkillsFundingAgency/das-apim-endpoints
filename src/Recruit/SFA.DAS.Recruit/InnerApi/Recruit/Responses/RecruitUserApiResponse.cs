using SFA.DAS.Recruit.Enums;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Responses;

public class RecruitUserApiResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("idamsUserId")]
    public string IdamsUserId { get; set; }

    [JsonPropertyName("dfEUserId")]
    public string DfEUserId { get; set; }

    [JsonPropertyName("userType")]
    public string UserType { get; set; }

    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }

    [JsonPropertyName("updatedDate")]
    public DateTime? UpdatedDate { get; set; }

    [JsonPropertyName("lastSignedInDate")]
    public DateTime? LastSignedInDate { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    public string? FirstName => Name?.Split(' ')[0];
    public string? LastName => Name?.Contains(' ') == true ? Name?[(Name.IndexOf(' ') + 1)..] : null;

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("notificationPreferences")]
    public NotificationPreferences NotificationPreferences { get; set; }
}

public class NotificationPreferences
{
    [JsonPropertyName("eventPreferences")]
    public List<EventPreference> EventPreferences { get; set; }
}

public class EventPreference
{
    [JsonPropertyName("event")]
    public NotificationTypes Event { get; set; }

    [JsonPropertyName("method")]
    public string Method { get; set; }

    [JsonPropertyName("scope")]
    public NotificationScope Scope { get; set; }

    [JsonPropertyName("frequency")]
    public NotificationFrequency Frequency { get; set; }
}