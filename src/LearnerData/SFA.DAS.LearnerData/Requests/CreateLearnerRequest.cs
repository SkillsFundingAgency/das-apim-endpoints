using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.LearnerData.Requests;

public class CreateLearnerRequest : UpdateLearnerRequest
{
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
        public new List<OnProgrammeRequestDetails> OnProgramme { get; set; }
    }
}


