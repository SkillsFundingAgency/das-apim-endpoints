using System;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.EmployerProfiles.Api.Models
{
    public class UpsertAccountRequest
    {
        [Required]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string GovIdentifier { get; set; }
        public Guid? CorrelationId { get; set; }
    }
}
