namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses
{
    public class GetStandardsListResponse
    {
        public IEnumerable<GetStandardsListItem> Standards { get; set; }
        public int TotalFiltered { get; set; }
        public int Total { get; set; }

        public class GetStandardsListItem
        {
            public string StandardUId { get; set; }
            public int LarsCode { get; set; }
            public string Title { get; set; }
            public int Level { get; set; }
            public string Route { get; set; }
            public int RouteCode { get; set; }
        }
    }
}
