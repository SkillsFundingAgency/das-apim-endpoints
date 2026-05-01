using SFA.DAS.Aodp.Validation;

namespace SFA.DAS.Aodp.Application.Queries.Qualifications
{
    public class GetQualificationsRequest
    {
        [AllowedValues("new", "changed")]
        public string? Status { get; set; }

        public int? Skip { get; set; }
        public int? Take { get; set; }

        [AllowedCharacters(TextCharacterProfile.Title)]
        public string? Name { get; set; }

        [AllowedCharacters(TextCharacterProfile.FreeText)]
        public string? Organisation { get; set; }

        [QualificationNumber]
        public string? Qan { get; set; }

        [AllowedCharacters(TextCharacterProfile.FreeText)]
        public string? ProcessStatusFilter { get; set; }
    }
}
