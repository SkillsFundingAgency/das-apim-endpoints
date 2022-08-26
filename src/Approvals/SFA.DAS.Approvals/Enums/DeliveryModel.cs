using System.Text.Json.Serialization;

namespace SFA.DAS.Approvals.InnerApi
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DeliveryModel : byte
    {
        Regular = 0,
        PortableFlexiJob = 1,
        FlexiJobAgency = 2
    }
}
