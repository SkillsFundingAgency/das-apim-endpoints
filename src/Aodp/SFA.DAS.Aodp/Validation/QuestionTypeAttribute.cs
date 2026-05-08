using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.Aodp.Validation
{
    public class QuestionTypeAttribute : AllowedValuesAttribute
    {
        public QuestionTypeAttribute() : base(
            "Text",
            "TextArea",
            "Number",
            "Date",
            "MultiChoice",
            "Radio",
            "File") { }

        protected override bool AllowNull => false;
    }
}
