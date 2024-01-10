namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses
{
    public class GetStandardsListResponse
    {
        public IEnumerable<GetStandardsListItem> Standards { get; set; } = null!;
        public int TotalFiltered { get; set; }
        public int Total { get; set; }

        public class GetStandardsListItem
        {
            public string StandardUId { get; set; } = null!;
            public int LarsCode { get; set; }
            public string Title { get; set; } = null!;
            public int Level { get; set; }
            public string Route { get; set; } = null!;
            public int RouteCode { get; set; }
        }
    }
}
