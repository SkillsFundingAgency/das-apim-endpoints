using Newtonsoft.Json;

namespace SFA.DAS.Vacancies.Manage.InnerApi.Responses
{
    public class GetEmployerAccountLegalEntityItem
    {
        [JsonProperty("Name")]
        public string AccountLegalEntityName { get; set; }
        public string AccountLegalEntityPublicHashedId { get; set; }
        public string AccountPublicHashedId { get ; set ; }
        public string AccountName { get ; set ; }
    }
}