using Newtonsoft.Json;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.InnerApi.Responses;
public record GetPagedApplicationReviewsByVacancyReferenceApiResponse
{
    [JsonProperty("info")]
    public PageInfo Info { get; set; }

    [JsonProperty("items")] 
    public List<Domain.ApplicationReview> Items { get; set; } = [];
}