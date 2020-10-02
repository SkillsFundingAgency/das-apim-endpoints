namespace SFA.DAS.Forecasting.InnerApi.Responses
{
    public class GetFrameworksListItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public decimal FundingCap { get; set; }
        public int Level { get; set; }
        public int Duration { get; set; }
    }
}