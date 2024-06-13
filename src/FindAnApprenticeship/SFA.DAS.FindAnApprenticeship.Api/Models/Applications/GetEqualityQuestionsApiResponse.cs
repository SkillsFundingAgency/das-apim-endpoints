using SFA.DAS.FindAnApprenticeship.Application.Queries.GetEqualityQuestions;
using SFA.DAS.FindAnApprenticeship.Domain.Models;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications
{
    public class GetEqualityQuestionsApiResponse
    {
        public EqualityQuestionsItem? EqualityQuestions { get; set; }

        public class EqualityQuestionsItem
        {
            public GenderIdentity? Sex { get; set; }
            public EthnicGroup? EthnicGroup { get; set; }
            public EthnicSubGroup? EthnicSubGroup { get; set; }
            public string? IsGenderIdentifySameSexAtBirth { get; set; }
            public string? OtherEthnicSubGroupAnswer { get; set; }
        }

        public static implicit operator GetEqualityQuestionsApiResponse(GetEqualityQuestionsQueryResult source)
        {
            if (source.EqualityQuestions == null)
                return new GetEqualityQuestionsApiResponse
                {
                    EqualityQuestions = null
                };

            return new GetEqualityQuestionsApiResponse
            {
                EqualityQuestions = new EqualityQuestionsItem
                {
                    Sex = source.EqualityQuestions.Sex,
                    EthnicSubGroup = source.EqualityQuestions.EthnicSubGroup,
                    EthnicGroup = source.EqualityQuestions.EthnicGroup,
                    IsGenderIdentifySameSexAtBirth = source.EqualityQuestions.IsGenderIdentifySameSexAtBirth,
                    OtherEthnicSubGroupAnswer = source.EqualityQuestions.OtherEthnicSubGroupAnswer
                }
            };
        }
    }
}
