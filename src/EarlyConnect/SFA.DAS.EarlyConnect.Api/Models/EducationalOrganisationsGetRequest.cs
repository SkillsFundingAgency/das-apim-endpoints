using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.EarlyConnect.Api.Requests.GetRequests
{
    public class EducationalOrganisationsGetRequest
    {
        [Required]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "Invalid Lep Code")]
        public string LepCode { get; set; }

        [RegularExpression(@"^[\w\s]+$", ErrorMessage = "Invalid Search Term")]
        public string? SearchTerm { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
