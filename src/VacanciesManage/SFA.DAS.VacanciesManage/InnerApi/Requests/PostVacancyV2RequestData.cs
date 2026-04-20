using System.Text.Json.Serialization;
using SFA.DAS.Recruit.Contracts.ApiResponses;

namespace SFA.DAS.VacanciesManage.InnerApi.Requests;

public class PostVacancyV2RequestData : PostVacancyRequest
{
    [JsonPropertyName("accountLegalEntityPublicHashedId")]
    public string AccountLegalEntityPublicHashedId { get; set; }
}