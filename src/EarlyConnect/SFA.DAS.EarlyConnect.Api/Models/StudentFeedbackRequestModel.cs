using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.EarlyConnect.Api.Models
{
    public class StudentFeedbackRequestModel
    {
        [Required]
        public Guid SurveyId { get; set; }

        [Required]
        [RegularExpression(@"^[\w\s]+$", ErrorMessage = "Invalid Status Update")]
        public string StatusUpdate { get; set; }

        [RegularExpression(@"^[\w\s]+$", ErrorMessage = "Invalid Notes")]
        public string Notes { get; set; }

        [Required]
        [RegularExpression(@"^[\w\s]+$", ErrorMessage = "Invalid Updated by User")]
        public string UpdatedBy { get; set; }
    }
}
