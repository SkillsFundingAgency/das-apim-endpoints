using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.CreateAccount.SignIntoYourOldAccount
{
    public class GetSignIntoYourOldAccountQuery : IRequest<GetSignIntoYourOldAccountQueryResult>
    {
        public Guid CandidateId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
