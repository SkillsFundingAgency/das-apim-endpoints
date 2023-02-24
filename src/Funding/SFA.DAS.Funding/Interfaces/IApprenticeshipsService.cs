using SFA.DAS.Funding.InnerApi.Responses;
using System.Threading.Tasks;

namespace SFA.DAS.Funding.Interfaces
{
    public interface IApprenticeshipsService
    {
        Task<ApprenticeshipsDto> GetAll(long ukprn);
    }
}