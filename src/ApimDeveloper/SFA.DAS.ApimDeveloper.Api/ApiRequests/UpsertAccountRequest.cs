using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ApimDeveloper.Api.ApiRequests
{
    public class UpsertAccountRequest
    {
        [Required]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string GovIdentifier { get; set; }
    }
}
