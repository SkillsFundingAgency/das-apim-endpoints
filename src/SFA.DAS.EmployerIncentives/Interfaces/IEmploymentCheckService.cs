using SFA.DAS.EmployerIncentives.InnerApi.Requests.EmploymentCheck;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Application.Commands.RegisterEmploymentCheck;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface IEmploymentCheckService
    {
        Task<RegisterEmploymentCheckResponse> Register(RegisterEmploymentCheckRequest request);
    }
}
