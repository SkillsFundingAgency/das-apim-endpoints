using System.Text.Json.Serialization;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Types
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ChangeOfPartyRequestType
    {
        ChangeEmployer = 0,
        ChangeProvider = 1
    }
}
