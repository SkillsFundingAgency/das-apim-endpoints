using System.ComponentModel;

namespace SFA.DAS.AdminAan.Domain;
public enum EventFormat
{
    [Description("In person")]
    InPerson,
    [Description("Online")]
    Online,
    [Description("Hybrid")]
    Hybrid
}
