using System.ComponentModel;

namespace SFA.DAS.Approvals.Enums
{
    public enum LearnerVerificationResponseCode
    {
        [Description("WSVRC001")]
        SuccessfulMatch,

        [Description("WSVRC002")]
        SuccessfulLinkedMatch,

        [Description("WSVRC003")]
        SimilarMatch,

        [Description("WSVRC004")]
        SimilarLinkedMatch,

        [Description("WSVRC005")]
        LearnerDoesNotMatch,

        [Description("WSVRC006")]
        UlnNotFound
    }
}