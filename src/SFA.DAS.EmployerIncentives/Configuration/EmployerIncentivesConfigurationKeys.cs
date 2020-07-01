using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.EmployerIncentives.Configuration
{
    public class EmployerIncentivesConfigurationKeys
    {
        public const string EmployerIncentivesOuterApi = "SFA.DAS.EmployerIncentives.OuterApi";
        public static string AzureActiveDirectoryApiConfiguration => $"{EmployerIncentivesOuterApi}:AzureAD";
    }
}
