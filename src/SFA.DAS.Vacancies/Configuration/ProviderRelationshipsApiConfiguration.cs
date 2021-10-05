using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Vacancies.Configuration
{
    public class ProviderRelationshipsApiConfiguration : IInternalApiConfiguration
    {
        public string Url { get; set; }
        public string Identifier { get; set; }
    }
}