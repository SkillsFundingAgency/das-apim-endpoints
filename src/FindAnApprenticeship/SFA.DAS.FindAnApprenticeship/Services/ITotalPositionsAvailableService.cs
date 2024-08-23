using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Services;

public interface ITotalPositionsAvailableService
{
    Task<long> GetTotalPositionsAvailable();
}