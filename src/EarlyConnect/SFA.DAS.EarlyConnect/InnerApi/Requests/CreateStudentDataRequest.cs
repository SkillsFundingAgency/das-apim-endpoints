using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.InnerApi.Requests
{
    public class CreateStudentDataRequest : IPostApiRequest
    {
        public object Data { get; set; }

        public CreateStudentDataRequest(StudentDataList studentDataList)
        {
            Data = studentDataList;
        }

        public string PostUrl => "api/student-data";
    }
}