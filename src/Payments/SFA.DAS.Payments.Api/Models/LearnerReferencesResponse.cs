using SFA.DAS.Payments.Models.Responses;

namespace SFA.DAS.Payments.Api.Models
{
    public class LearnerReferenceResponse
    {
        public int Uln { get; set; }
        public string LearnerReferenceNumber { get; set; } = string.Empty;
    }

    public static class LearnerReferenceResponseExtensions
    {
        public static IEnumerable<LearnerReferenceResponse> ToLearnerReferenceResponse(this IEnumerable<LearnerResponse> learners)
        {
            return learners.Select(x=> new LearnerReferenceResponse { Uln = x.Uln, LearnerReferenceNumber = x.LearnRefNumber });
        }
    }
}
