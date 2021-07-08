using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface IVendorRegistrationService
    {
        Task UpdateVendorRegistrationCaseStatus(UpdateVendorRegistrationCaseStatusRequest request);
        Task AddEmployerVendorIdToLegalEntity(string hashedLegalEntityId, string employerVendorId);
        Task<GetLatestVendorRegistrationCaseUpdateDateTimeResponse> GetLatestVendorRegistrationCaseUpdateDateTime();
    }
}
