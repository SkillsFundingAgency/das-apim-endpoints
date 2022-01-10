using System.Threading.Tasks;
using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Interfaces
{
    public interface IStandardsService
    {
        Task<GetStandardsListResponse> GetStandards();
    }
}