using SFA.DAS.EmployerIncentives.InnerApi.Requests.EmploymentCheck;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface IIncentivesEmploymentCheckService
    {
        Task Update(UpdateRequest updateRequest);
    }
}
