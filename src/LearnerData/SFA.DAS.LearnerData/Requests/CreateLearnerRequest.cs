namespace SFA.DAS.LearnerData.Requests;

public class CreateLearnerRequest : UpdateLearnerRequest
{
    public new LearnerDetails Learner { get; set; }

    public new DeliveryDetails Delivery { get; set; }

    public class LearnerDetails : LearnerRequestDetails
    {
    }

    public class DeliveryDetails : UpdateLearnerRequestDeliveryDetails
    {
        public new List<OnProgrammeDetails> OnProgramme { get; set; }
    }

    public class OnProgrammeDetails : OnProgrammeRequestDetails
    {
    }
}


