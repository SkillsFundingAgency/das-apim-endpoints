using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerAccounts.Application.Interfaces
{
    public interface  IEmployerAccountsService
    {
        Task<bool> IsHealthy();
    }
}
