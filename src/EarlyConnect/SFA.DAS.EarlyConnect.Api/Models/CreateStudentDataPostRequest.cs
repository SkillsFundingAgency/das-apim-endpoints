namespace SFA.DAS.EarlyConnect.Api.Models
{
    public class CreateStudentDataPostRequest
    {
        public IEnumerable<StudentRequestModel> ListOfStudentData { get; set; }
    }
}
