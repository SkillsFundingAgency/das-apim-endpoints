using System.Text.Json.Serialization;

namespace SFA.DAS.EmployerFinanceJobs.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PaymentStatus : short
{
    Active = 1,
    Paused = 2,
    Withdrawn = 3,
    Completed = 4
}