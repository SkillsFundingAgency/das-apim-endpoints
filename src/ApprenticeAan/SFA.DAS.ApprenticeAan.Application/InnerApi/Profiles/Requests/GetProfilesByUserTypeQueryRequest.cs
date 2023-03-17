using SFA.DAS.ApprenticeAan.Application.Common;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeAan.Application.InnerApi.Profiles.Requests
{
    public class GetProfilesByUserTypeQueryRequest : IGetApiRequest
    {
        public string GetUrl => Constants.AanHubApiUrls.GetProfilesUrl + UserType;
        private string UserType { get; }
        public GetProfilesByUserTypeQueryRequest(string userType) => UserType = userType;
    }
}