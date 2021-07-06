using SFA.DAS.LevyTransferMatching.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserAccount>> GetUserAccounts(string userId);
    }
}