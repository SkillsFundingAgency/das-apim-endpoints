using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.Reservations.Application.Accounts;

public class User
{
    public string UserRef { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Role { get; set; }

    public bool CanReceiveNotifications { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public InvitationStatus Status { get; set; }
}