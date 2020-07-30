using System.ComponentModel;

namespace SFA.DAS.EmployerIncentives.Models.EmployerIncentives
{
    public enum ApprenticeshipEmployerType : byte
    {
        [Description("Non Levy")] NonLevy,
        [Description("Levy")] Levy,
        [Description("Unknown")] Unknown,
    }
}
