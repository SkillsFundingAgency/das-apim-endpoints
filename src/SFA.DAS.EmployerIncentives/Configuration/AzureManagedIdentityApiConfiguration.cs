using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Configuration
{
    public class AzureManagedIdentityApiConfiguration
    {
        public string Tenant { get; set; }
        public string Identifier { get; set; }
        public string Url { get; set; }
    }
}
