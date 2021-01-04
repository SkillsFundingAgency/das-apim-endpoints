using System.Text.RegularExpressions;

namespace SFA.DAS.SharedOuterApi.Validation
{
    public abstract class EpaoIdValidator
    {
        protected void ValidateEpaoId(string epaoId, ref ValidationResult result)
        {
            var regex = new Regex(@"[eE][pP][aA][0-9]{4,9}$"); // Note: as per validation rule in assessor service
            if (epaoId == null || !regex.Match(epaoId).Success)
            {
                result.AddError(nameof(epaoId), $"Invalid {nameof(epaoId)}");
            }
        }
    }
}