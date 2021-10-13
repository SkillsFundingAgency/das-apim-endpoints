using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Vacancies.Manage.Configuration
{
    public class ProviderRelationshipsApiConfiguration : IInternalApiConfiguration
    {
        public string Url { get; set; }
        public string Identifier { get; set; }
    }
}