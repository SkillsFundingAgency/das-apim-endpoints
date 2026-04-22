using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class GetLatestVendorRegistrationCaseUpdateDateTimeRequest : IGetApiRequest
    {
        public string GetUrl => "accounts/last-vrf-update-date";
    }
}