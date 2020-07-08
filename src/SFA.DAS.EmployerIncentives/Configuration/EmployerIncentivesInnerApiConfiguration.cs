using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Configuration
{
    public class EmployerIncentivesInnerApiConfiguration : IAzureManagedIdentityApiConfiguration
    {
        public string Tenant { get; set; }
        public string Identifier { get; set; }
        public string Url { get; set; }
    }
}
