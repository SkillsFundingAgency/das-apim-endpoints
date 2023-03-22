using SFA.DAS.ApprenticePortal.InnerApi.ApprenticeAccounts.Requests;
using System.Threading.Tasks;


namespace SFA.DAS.ApprenticePortal.Interfaces
{
    public interface IApprenticePortalService
    {
        Task UpdateApprentice(UpdateApprenticeRequest request);
    }
}