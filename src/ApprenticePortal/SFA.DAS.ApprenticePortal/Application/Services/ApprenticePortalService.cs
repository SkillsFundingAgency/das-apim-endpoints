using SFA.DAS.ApprenticePortal.Configuration;
using SFA.DAS.ApprenticePortal.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.ApprenticePortal.Interfaces;
using System.Threading.Tasks;


namespace SFA.DAS.EmployerIncentives.Application.Services
{
    public class ApprenticePortalService : IApprenticePortalService
    {
        private readonly IApprenticePortalApiClient<ApprenticePortalConfiguration> _client;

        public ApprenticePortalService(IApprenticePortalApiClient<ApprenticePortalConfiguration> client)
        {
            _client = client;
        }

        public async Task UpdateApprentice(UpdateApprenticeRequest request)
        {
            await _client.Patch(new PatchUpdateApprenticeRequest { Data = request });
        }
    }
}
