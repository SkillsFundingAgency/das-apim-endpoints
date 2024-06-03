using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.CreateAccount.SignIntoYourOldAccount;

public class GetSignIntoYourOldAccountQueryHandler : IRequestHandler<GetSignIntoYourOldAccountQuery, GetSignIntoYourOldAccountQueryResult>
{
    public Task<GetSignIntoYourOldAccountQueryResult> Handle(GetSignIntoYourOldAccountQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}