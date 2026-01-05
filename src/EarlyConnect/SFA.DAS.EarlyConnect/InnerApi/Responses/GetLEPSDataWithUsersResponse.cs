namespace SFA.DAS.EarlyConnect.InnerApi.Responses
{
    public class GetLEPSDataWithUsersResponse
    {
        public ICollection<LEPSData>? LEPSData { get; set; }
    }
    public class LEPSData
    {
        public int Id { get; set; }
        public string LepCode { get; set; }
        public string Region { get; set; }
        public string LepName { get; set; }
        public string EntityEmail { get; set; }
        public string Postcode { get; set; }
        public DateTime DateAdded { get; set; }
        public ICollection<LEPSUser>? LEPSUsers { get; set; }
    }
    public class LEPSUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
