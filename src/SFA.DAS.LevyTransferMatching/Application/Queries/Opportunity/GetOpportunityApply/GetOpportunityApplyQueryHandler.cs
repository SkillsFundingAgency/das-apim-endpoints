using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetOpportunityApply
{
    public class GetOpportunityApplyQueryHandler : IRequestHandler<GetOpportunityApplyQuery, GetOpportunityApplyQueryResult>
    {
        private readonly IUserService _userService;

        public GetOpportunityApplyQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<GetOpportunityApplyQueryResult> Handle(GetOpportunityApplyQuery request, CancellationToken cancellationToken)
        {
            var userAccounts = await _userService.GetUserAccounts(request.UserId);

            return new GetOpportunityApplyQueryResult()
            {
                Accounts = userAccounts.Select(x => new GetOpportunityApplyQueryResult.Account()
                {
                    EncodedAccountId = x.EncodedAccountId,
                    Name = x.DasAccountName,
                }),
            };
        }
    }
}