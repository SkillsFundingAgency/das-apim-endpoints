using System.ComponentModel;

namespace SFA.DAS.AdminAan.Infrastructure;
public enum EventFormat
{
    [Description("In person")]
    InPerson,
    [Description("Online")]
    Online,
    [Description("Hybrid")]
    Hybrid
}
