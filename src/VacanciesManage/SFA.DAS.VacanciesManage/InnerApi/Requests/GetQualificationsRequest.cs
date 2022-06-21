using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.VacanciesManage.InnerApi.Requests
{
    public class GetQualificationsRequest : IGetApiRequest
    {
        public string GetUrl => "api/referencedata/candidate-qualifications";
    }
}