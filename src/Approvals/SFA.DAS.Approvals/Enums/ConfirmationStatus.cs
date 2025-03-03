using System.ComponentModel;

namespace SFA.DAS.Approvals.Enums
{
    public enum ConfirmationStatus : short
    {
        [Description("N/A")]
        NA = 4,
        Overdue = 3,
        Unconfirmed = 2,
        Confirmed = 1
    }
}
