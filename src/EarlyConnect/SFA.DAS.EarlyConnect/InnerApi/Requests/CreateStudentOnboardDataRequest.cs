using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.InnerApi.Requests
{
    public class CreateStudentOnboardDataRequest : IPostApiRequest
    {
        public object Data { get; set; }

        public CreateStudentOnboardDataRequest(EmailData emails)
        {
            Data = emails;
        }
        public string PostUrl => "api/student-data/onboard";
    }
}