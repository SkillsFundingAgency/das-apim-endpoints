namespace SFA.DAS.LearnerData.Services;

public interface IFundingBandMaximumService
{
    Task<int> GetFundingBandMaximum(int standardCode, DateTime effectiveDate);
}