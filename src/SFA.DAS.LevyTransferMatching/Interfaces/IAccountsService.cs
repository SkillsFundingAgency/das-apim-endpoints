﻿using SFA.DAS.LevyTransferMatching.Models;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Interfaces
{
    public interface IAccountsService
    {
        Task<Account> GetAccount(string encodedAccountId);
    }
}