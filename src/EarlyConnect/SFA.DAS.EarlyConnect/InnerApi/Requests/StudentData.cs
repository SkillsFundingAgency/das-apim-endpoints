namespace SFA.DAS.EarlyConnect.InnerApi.Requests
{
    public class StudentDataList 
    {
        public IEnumerable<StudentData> ListOfStudentData { get; set; }
    }
    public class StudentData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Postcode { get; set; }
        public string Industry { get; set; }
        public DateTime DateOfInterest { get; set; }
    }
}
