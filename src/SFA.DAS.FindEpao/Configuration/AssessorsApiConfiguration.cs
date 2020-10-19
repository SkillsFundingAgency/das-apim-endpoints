using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindEpao.Configuration
{
    public class AssessorsApiConfiguration : IInternalApiConfiguration
    {
        public string Url { get; set; }
        public string Identifier { get; set; }
    }
}