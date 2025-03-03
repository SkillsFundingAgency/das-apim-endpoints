using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications
{
    public class GetNewQualificationsApiRequest : IGetApiRequest
    {
        public string GetUrl => "api/qualifications?status=new";
    }

}
