using System.Threading.Tasks;

namespace SFA.DAS.Funding.Interfaces
{
    public interface IFundingApprenticeshipEarningsService
    {
        Task<bool> IsHealthy();
    }
}