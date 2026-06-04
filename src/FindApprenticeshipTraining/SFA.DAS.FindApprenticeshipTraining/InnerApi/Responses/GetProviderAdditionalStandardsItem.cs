using SFA.DAS.SharedOuterApi.Types.Constants;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetProviderAdditionalStandardsItem
    {
        public string LarsCode { get; set; }
        public string CourseName { get; set; }
        public CourseType CourseType { get; set; }
        public LearningType ApprenticeshipType { get; set; }
        public int Level { get; set; }
        public bool? IsApprovedByRegulator { get; set; }
        public string ApprovalBody { get; set; }
        public string IfateReferenceNumber { get; set; }
    }
}