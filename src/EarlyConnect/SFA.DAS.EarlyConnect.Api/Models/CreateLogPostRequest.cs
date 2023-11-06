namespace SFA.DAS.EarlyConnect.Api.Models
{
    public class CreateLogPostRequest
    {
        public string RequestType { get; set; }
        public string RequestSource { get; set; }
        public string RequestIP { get; set; }
        public string Payload { get; set; }
        public string FileName { get; set; }
        public string Status { get; set; }
        public string Error { get; set; }
    }
}
