using SFA.DAS.Funding.InnerApi.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.Funding.Interfaces
{
    public interface IApprenticeshipsService
    {
        Task<IEnumerable<ApprenticeshipDto>> GetAll(long ukprn);
    }
}