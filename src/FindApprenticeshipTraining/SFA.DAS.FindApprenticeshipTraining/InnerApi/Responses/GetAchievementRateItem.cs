namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetAchievementRateItem
    {
        public string Level { get; set; }
        public string SectorSubjectArea { get; set; }
        public int OverallCohort { get; set; }
        public decimal OverallAchievementRate { get; set; }
    }
}