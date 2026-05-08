using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.Aodp.Validation
{
    public class MessageTypeAttribute : AllowedValuesAttribute
    {
        public MessageTypeAttribute() : base(
            "UnlockApplication",
            "PutApplicationOnHold",
            "RequestInformationFromAOByQfau",
            "RequestInformationFromAOByOfqaul",
            "RequestInformationFromAOBySkillsEngland",
            "ReplyToInformationRequest",
            "InternalNotes",
            "InternalNotesForQfauFromOfqual",
            "InternalNotesForQfauFromSkillsEngland",
            "InternalNotesForPartners",
            "ApplicationSharedWithOfqual",
            "ApplicationSharedWithSkillsEngland",
            "ApplicationUnsharedWithOfqual",
            "ApplicationUnsharedWithSkillsEngland",
            "ApplicationSubmitted") { }

        protected override bool AllowNull => false;
    }
}
