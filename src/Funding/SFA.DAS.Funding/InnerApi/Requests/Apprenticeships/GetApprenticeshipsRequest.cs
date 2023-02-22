using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Funding.InnerApi.Requests.Apprenticeships
{
    public class GetApprenticeshipsRequest : IGetAllApiRequest
    {
        private readonly long _ukprn;

        public GetApprenticeshipsRequest(long ukprn)
        {
            _ukprn = ukprn;
        }

        public string GetAllUrl => $"{_ukprn}/apprenticeships";
    }
}