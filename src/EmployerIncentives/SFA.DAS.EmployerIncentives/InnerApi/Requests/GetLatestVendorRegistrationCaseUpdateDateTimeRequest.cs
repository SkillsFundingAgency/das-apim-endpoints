using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class GetLatestVendorRegistrationCaseUpdateDateTimeRequest : IGetApiRequest
    {
        public string GetUrl => "accounts/last-vrf-update-date";
    }
}