using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.Configuration
{
    public class EmployerIncentivesConfiguration : IInnerApiConfiguration
    {
        public string Url { get; set; }
        public string Identifier { get; set; }
    }
}