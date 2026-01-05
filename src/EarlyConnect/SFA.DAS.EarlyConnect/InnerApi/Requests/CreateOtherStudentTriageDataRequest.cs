using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.InnerApi.Requests
{
    public class CreateOtherStudentTriageDataRequest : IPostApiRequest
    {
        public object Data { get; set; }

        public CreateOtherStudentTriageDataRequest(OtherStudentTriageData otherStudentTriageData)
        {
            Data = otherStudentTriageData;
        }

        public string PostUrl => "api/student-triage-data/survey-create";
    }
}