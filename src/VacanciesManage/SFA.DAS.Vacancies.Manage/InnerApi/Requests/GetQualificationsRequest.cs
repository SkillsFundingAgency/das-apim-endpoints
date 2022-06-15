using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Vacancies.Manage.InnerApi.Requests
{
    public class GetQualificationsRequest : IGetApiRequest
    {
        public string GetUrl => "api/referencedata/candidate-qualifications";
    }
}