using System.Threading.Tasks;
using SFA.DAS.Funding.InnerApi.Responses;

namespace SFA.DAS.Funding.Interfaces
{
    public interface IFundingProviderEarningsService
    {
        Task<ProviderEarningsSummaryDto> GetSummary(long ukprn);
        Task<AcademicYearEarningsDto> GetAcademicYearEarnings(long ukprn);
    }
}