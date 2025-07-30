using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.Recruit.InnerApi.Responses;
public record GetDashboardVacanciesCountApiResponse
{
    [JsonProperty("info")]
    public PageInfo Info { get; set; }

    [JsonProperty("items")] 
    public List<Item> Items { get; set; } = [];

    public record Item
    {
        public int VacancyReference { get; set; } = 0;
        public int NewApplications { get; set; } = 0;
        public int Applications { get; set; } = 0;
        public int Shared { get; set; } = 0;
        public int AllSharedApplications { get; set; } = 0;
    }
}
