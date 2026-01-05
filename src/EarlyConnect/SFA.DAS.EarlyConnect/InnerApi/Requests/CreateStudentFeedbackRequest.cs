using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.InnerApi.Requests
{
    public class CreateStudentFeedbackRequest : IPostApiRequest
    {
        public object Data { get; set; }

        public CreateStudentFeedbackRequest(StudentFeedbackList studentFeedbackList)
        {
            Data = studentFeedbackList;
        }

        public string PostUrl => "api/leps-data/student-feedback";
    }
}
