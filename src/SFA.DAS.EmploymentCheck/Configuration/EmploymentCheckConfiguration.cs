using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmploymentCheck.Configuration
{
    public class EmploymentCheckConfiguration : IInternalApiConfiguration
    {
        public string Url { get; set; }
        public string Identifier { get; set; }
    }
}