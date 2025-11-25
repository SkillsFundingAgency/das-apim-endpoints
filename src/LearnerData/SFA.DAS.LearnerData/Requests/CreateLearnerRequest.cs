using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.LearnerData.Requests;

public class CreateLearnerRequest : UpdateLearnerRequest
{
    public string ConsumerReference { get; set; }
    public new LearnerDetails Learner { get; set; }

    public new DeliveryDetails Delivery { get; set; }

    public class LearnerDetails : LearnerRequestDetails
    {
        [Required]
        public long Uln { get; set; }
        [Required]
        public string LearnerRef { get; set; }
        [Required]
        public DateTime? Dob { get; set; }
        [Required]
        public bool? HasEhcp { get; set; }
    }

    public class DeliveryDetails : UpdateLearnerRequestDeliveryDetails
    {
        public new List<OnProgrammeDetails> OnProgramme { get; set; }
    }

    public class OnProgrammeDetails : OnProgrammeRequestDetails
    {
        public int PercentageOfTrainingLeft { get; set; }
        public bool? IsFlexiJob { get; set; }
        public int? StandardCode { get; set; }
        public string? AgreementId { get; set; }
    }
}


