using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.CreateAccount.CheckAnswers
{
    public class GetCheckAnswersQuery : IRequest<GetCheckAnswersQueryResult>
    {
        public Guid CandidateId { get; set; }
    }
}
