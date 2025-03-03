using System;
using System.ComponentModel;

namespace SFA.DAS.Approvals.Enums
{
    [Flags]
    public enum Alerts
    {
        [Description("ILR data mismatch")]
        IlrDataMismatch = 0,
        [Description("Changes pending")]
        ChangesPending = 1,
        [Description("Changes requested")]
        ChangesRequested = 2,
        [Description("Changes for review")]
        ChangesForReview = 3,
        [Description("Confirm dates")]
        ConfirmDates = 4
    }
}
