using System.ComponentModel;

namespace SFA.DAS.Approvals.Enums
{
    public enum FailureFlag
    {
        [Description("VRF1")]
        GivenDoesntMatchGiven,

        [Description("VRF2")]
        GivenDoesntMatchFamily,

        [Description("VRF3")]
        GivenDoesntMatchPreviousFamily,

        [Description("VRF4")]
        FamilyDoesntMatchGiven,

        [Description("VRF5")]
        FamilyDoesntMatchFamily,

        [Description("VRF6")]
        FamilyDoesntMatchPreviousFamily,

        [Description("VRF7")]
        DateOfBirthDoesntMatchDateOfBirth,

        [Description("VRF8")]
        GenderDoesntMatchGender
    }
}