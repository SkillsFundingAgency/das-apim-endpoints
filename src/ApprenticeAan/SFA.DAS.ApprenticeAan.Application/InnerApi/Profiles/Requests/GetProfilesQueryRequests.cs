using SFA.DAS.ApprenticeAan.Application.Common;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeAan.Application.InnerApi.Profiles.Requests
{
    public class GetProfilesQueryRequest : IGetApiRequest
    {
        public string GetUrl => Constants.AanHubApiUrls.GetProfilesUrl;
        private string UserType { get; }
        public GetProfilesQueryRequest(string userType) => UserType = userType;

        //remove this and use the above one with userType
        public GetProfilesQueryRequest()
        {

        }
    }
}
