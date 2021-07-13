using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;

namespace SFA.DAS.LevyTransferMatching.Interfaces
{
    public interface IReferenceDataService
    {
        Task<List<ReferenceDataItem>> GetLevels();
        Task<List<ReferenceDataItem>> GetSectors();
        Task<List<ReferenceDataItem>> GetJobRoles();
    }
}
