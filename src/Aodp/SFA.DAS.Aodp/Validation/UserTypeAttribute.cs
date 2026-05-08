using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.Aodp.Validation
{
    public class UserTypeAttribute : AllowedValuesAttribute
    {
        public UserTypeAttribute() : base(
            "AwardingOrganisation",
            "SkillsEngland",
            "Ofqual",
            "Qfau") { }

        protected override bool AllowNull => false;
    }
}
