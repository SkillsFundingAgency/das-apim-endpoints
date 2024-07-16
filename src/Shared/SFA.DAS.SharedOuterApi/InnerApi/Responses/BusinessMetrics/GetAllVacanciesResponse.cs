using Newtonsoft.Json;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.BusinessMetrics
{
    public record GetAllVacanciesResponse
    {
        [JsonProperty("vacancies")]
        public List<string> Vacancies { get; set; }
    }
}
