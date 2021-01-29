using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.Services.ApprenticeLogin
{
    public interface IApprenticeLoginService
    {
        Task<bool> IsHealthy();
        Task SendInvitation(Guid guid, string email);
    }
}