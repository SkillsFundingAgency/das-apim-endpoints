using System;
using System.ComponentModel;

namespace SFA.DAS.Approvals.Enums;

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
    ConfirmDates = 4,
    [Description("ILR change invalid")]
    IlrChangeInvalid = 5,
    [Description("Changes declined")]
    ChangesDeclined = 6,
    [Description("ILR changes pending")]
    IlrChangesPending = 7
}