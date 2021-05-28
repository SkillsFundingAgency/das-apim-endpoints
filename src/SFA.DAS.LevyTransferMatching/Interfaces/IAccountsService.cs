using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Interfaces
{
    public interface IAccountsService
    {
        Task GetTransferAllowance(string accountId);
    }
}
