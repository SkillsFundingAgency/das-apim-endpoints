using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;

namespace SFA.DAS.ApprenticeFeedback.Models
{
    public class ApprenticeshipDetails
    {
        public InnerApi.Requests.LearnerData LearnerData { get; set; }
        public MyApprenticeshipData MyApprenticeshipData { get; set; }
    }
}
