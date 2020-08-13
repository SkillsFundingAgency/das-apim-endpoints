using System.ComponentModel;

namespace SFA.DAS.EmployerIncentives.InnerApi
{
    public enum ApprenticeshipEmployerType : byte
    {
        [Description("Non Levy")] NonLevy,
        [Description("Levy")] Levy,
        [Description("Unknown")] Unknown,
    }
}
