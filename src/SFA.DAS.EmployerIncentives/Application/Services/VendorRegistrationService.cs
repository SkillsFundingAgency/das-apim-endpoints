using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Services
{
    public class VendorRegistrationService : IVendorRegistrationService
    {
        private readonly IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> _client;

        public VendorRegistrationService(IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> client)
        {
            _client = client;
        }
        public async Task UpdateVendorRegistrationCaseStatus(UpdateVendorRegistrationCaseStatusRequest request)
        {
            await _client.Patch(new PatchVendorRegistrationCaseStatusRequest(request));
        }

        public Task AddEmployerVendorIdToLegalEntity(string hashedLegalEntityId, string employerVendorId)
        {
            return _client.Put(new PutEmployerVendorIdForLegalEntityRequest(hashedLegalEntityId)
                { Data = new PutEmployerVendorIdForLegalEntityRequestData { EmployerVendorId = employerVendorId } });
        }
        
        public async Task<GetLatestVendorRegistrationCaseUpdateDateTimeResponse> GetLatestVendorRegistrationCaseUpdateDateTime()
        {
            return await _client.Get<GetLatestVendorRegistrationCaseUpdateDateTimeResponse>(new GetLatestVendorRegistrationCaseUpdateDateTimeRequest());
        }
    }
}
