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
    public string Event { get; set; }

    [JsonPropertyName("method")]
    public string Method { get; set; }

    [JsonPropertyName("scope")]
    public string Scope { get; set; }

    [JsonPropertyName("frequency")]
    public string Frequency { get; set; }
}