using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SFA.DAS.LearnerData.Requests;

public class CreateLearnerRequest : UpdateLearnerRequest
{
    public string ConsumerReference { get; set; }
    public LearnerDetails Learner { get; set; }

    public new DeliveryDetails Delivery { get; set; }
    

    public class LearnerDetails
    {
        [Required]
        public string Uln { get; set; }
        [Required]
        public string LearnerRef { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public DateTime? Dob { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public bool? HasEhcp { get; set; }
    }

    public class DeliveryDetails : UpdateLearnerRequestDeliveryDetails
    {
        public new OnProgrammeDetails OnProgramme { get; set; }
    }

    public class OnProgrammeDetails : OnProgrammeRequestDetails
    {
        public int PercentageOfTrainingLeft { get; set; }
        public DateTime? StartDate { get; set; }
        public bool? IsFlexiJob { get; set; }
        public int? StandardCode { get; set; }
        public string? AgreementId { get; set; }
    }

    public class MathsAndEnglishDetails : MathsAndEnglish
    {
        public string CourseCode { get; set; } = "";
    }
}


