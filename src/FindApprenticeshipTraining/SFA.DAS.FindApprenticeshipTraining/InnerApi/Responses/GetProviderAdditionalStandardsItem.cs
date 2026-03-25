using SFA.DAS.Apim.Shared.Common;
using SFA.DAS.SharedOuterApi.Types.Constants;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetProviderAdditionalStandardsItem
    {
        public string LarsCode { get; set; }
        public string CourseName { get; set; }
        public LearningType ApprenticeshipType { get; set; }
        public int Level { get; set; }
        public bool? IsApprovedByRegulator { get; set; }
        public string ApprovalBody { get; set; }
        public string IfateReferenceNumber { get; set; }
    }
}