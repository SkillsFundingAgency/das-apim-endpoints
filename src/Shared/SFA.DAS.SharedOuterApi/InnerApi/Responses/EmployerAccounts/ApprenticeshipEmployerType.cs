using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ApprenticeshipEmployerType : byte
{
    NonLevy = 0,
    Levy = 1,
    Unknown = 2,
}