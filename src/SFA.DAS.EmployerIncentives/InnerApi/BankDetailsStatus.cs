using System.ComponentModel;

namespace SFA.DAS.EmployerIncentives.InnerApi
{
    public enum BankDetailsStatus : byte
    {
        [Description("Not Supplied")] NotSupplied,
        [Description("In Progress")] InProgress,
        [Description("Rejected")] Rejected,
        [Description("Completed")] Completed
    }
}
