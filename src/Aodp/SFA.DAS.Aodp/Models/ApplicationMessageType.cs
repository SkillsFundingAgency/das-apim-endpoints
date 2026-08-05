namespace SFA.DAS.Aodp.Models
{
    public enum ApplicationMessageType
    {
        UnlockApplication,
        PutApplicationOnHold,

        RequestInformationFromAOByQfau,
        RequestInformationFromAOByOfqaul,
        RequestInformationFromAOBySkillsEngland,

        ReplyToInformationRequest,

        InternalNotes,

        InternalNotesForQfauFromOfqual,
        InternalNotesForQfauFromSkillsEngland,

        InternalNotesForPartners,

        // System Audit
        ApplicationSharedWithOfqual,
        ApplicationSharedWithSkillsEngland,

        ApplicationUnsharedWithOfqual,
        ApplicationUnsharedWithSkillsEngland,

        ApplicationSubmitted,
        ApplicationWithdrawn,

        OfqualFeedbackSubmitted,
        SkillsEnglandFeedbackSubmitted,

        QfauOwnerUpdated,
        SkillsEnglandOwnerUpdated,
        OfqualOwnerUpdated,

        QanUpdated,

        AoInformedOfDecision
    }
}
