namespace SFA.DAS.RecruitQa.InnerApi.Responses;

public record GetFrameworksListItem
{
    public string Id { get; set; }
    public string Title { get; set; }
    public int CurrentFundingCap { get; set; }
    public int Level { get; set; }
    public int Duration { get; set; }
    public List<FundingPeriod> FundingPeriods { get; set; }
    public bool IsActiveFramework { get; set; }
    public string FrameworkName { get ; set ; }
    public string PathwayName { get ; set ; }
    public int PathwayCode { get ; set ; }
    public int FrameworkCode { get ; set ; }
    public int ProgType { get ; set ; }
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public int Ssa1 { get; set; }

    public record FundingPeriod
    {
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int FundingCap { get; set; }
    }
}