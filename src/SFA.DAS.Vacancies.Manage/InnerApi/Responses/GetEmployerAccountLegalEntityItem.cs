using Newtonsoft.Json;

namespace SFA.DAS.Vacancies.Manage.InnerApi.Responses
{
    public class GetEmployerAccountLegalEntityItem
    {
        // subset of properties taken from das-employerapprenticeshipsservice\src\SFA.DAS.EmployerAccounts.Api.Types\LegalEntity.cs
        public string Address { get; set; }
        [JsonProperty("Name")]
        public string AccountLegalEntityName { get; set; }
        public string AccountLegalEntityPublicHashedId { get; set; }
        public string AccountPublicHashedId { get ; set ; }
        public string AccountName { get ; set ; }
    }
}