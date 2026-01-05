namespace SFA.DAS.EarlyConnect.Api.Models
{
    public class UpdateLogPostRequest
    {
        public int LogId { get; set; }
        public string Status { get; set; }
        public string Error { get; set; }
    }
}
