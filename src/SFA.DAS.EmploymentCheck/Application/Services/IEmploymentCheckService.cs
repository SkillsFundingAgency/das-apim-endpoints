using System.Threading.Tasks;

namespace SFA.DAS.EmploymentCheck.Application.Services
{
    public interface IEmploymentCheckService
    {
        Task<bool> IsHealthy();
    }
}
