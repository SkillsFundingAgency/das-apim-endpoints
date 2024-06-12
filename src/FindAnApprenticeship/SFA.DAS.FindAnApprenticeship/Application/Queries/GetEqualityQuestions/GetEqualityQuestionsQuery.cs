using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetEqualityQuestions
{
    public class GetEqualityQuestionsQuery : IRequest<GetEqualityQuestionsQueryResult>
    {
        public Guid CandidateId { get; set; }
    }

    public class GetEqualityQuestionsQueryResult
    {
        public GenderIdentity? Sex { get; set; }
        public EthnicGroup? EthnicGroup { get; set; }
        public EthnicSubGroup? EthnicSubGroup { get; set; }
        public string? IsGenderIdentifySameSexAtBirth { get; set; }
        public string? OtherEthnicSubGroupAnswer { get; set; }
    }

    public class GetEqualityQuestionsQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient) : IRequestHandler<GetEqualityQuestionsQuery, GetEqualityQuestionsQueryResult>
    {
        public async Task<GetEqualityQuestionsQueryResult> Handle(GetEqualityQuestionsQuery request, CancellationToken cancellationToken)
        {
            var aboutYouResponse = await candidateApiClient.Get<GetAboutYouItemApiResponse>(new GetAboutYouItemApiRequest(request.CandidateId));

            return new GetEqualityQuestionsQueryResult
            {
                Sex = aboutYouResponse?.AboutYou?.Sex,
                EthnicGroup = aboutYouResponse?.AboutYou?.EthnicGroup,
                EthnicSubGroup = aboutYouResponse?.AboutYou?.EthnicSubGroup,
                IsGenderIdentifySameSexAtBirth = aboutYouResponse?.AboutYou?.IsGenderIdentifySameSexAtBirth,
                OtherEthnicSubGroupAnswer = aboutYouResponse?.AboutYou?.OtherEthnicSubGroupAnswer
            };
        }
    }
}
