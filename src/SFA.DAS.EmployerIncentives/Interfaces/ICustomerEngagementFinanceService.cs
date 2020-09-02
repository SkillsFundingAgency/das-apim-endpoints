using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.VendorRegistrationForm;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface ICustomerEngagementFinanceService
    {
        Task<GetVendorByApprenticeshipLegalEntityIdResponse> GetVendorByApprenticeshipLegalEntityId(string companyName, string hashedLegalEntityId);
    }
}