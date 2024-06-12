using SFA.DAS.FindAnApprenticeship.Application.Queries.GetEqualityQuestions;
using SFA.DAS.FindAnApprenticeship.Domain.Models;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications
{
    public class GetEqualityQuestionsApiResponse
    {
        public GenderIdentity? Sex { get; set; }
        public EthnicGroup? EthnicGroup { get; set; }
        public EthnicSubGroup? EthnicSubGroup { get; set; }
        public string? IsGenderIdentifySameSexAtBirth { get; set; }
        public string? OtherEthnicSubGroupAnswer { get; set; }

        public static implicit operator GetEqualityQuestionsApiResponse(GetEqualityQuestionsQueryResult source)
        {
            return new GetEqualityQuestionsApiResponse
            {
                Sex = source.Sex,
                EthnicSubGroup = source.EthnicSubGroup,
                EthnicGroup = source.EthnicGroup,
                IsGenderIdentifySameSexAtBirth = source.IsGenderIdentifySameSexAtBirth,
                OtherEthnicSubGroupAnswer = source.OtherEthnicSubGroupAnswer
            };
        }
    }
}
