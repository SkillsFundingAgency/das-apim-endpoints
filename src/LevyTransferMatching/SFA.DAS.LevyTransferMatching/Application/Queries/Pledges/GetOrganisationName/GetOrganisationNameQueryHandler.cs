using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetOrganisationName
{
    public class GetOrganisationNameQueryHandler : IRequestHandler<GetOrganisationNameQuery, GetOrganisationNameQueryResult>
    {
        private readonly IAccountsService _accountsService;

        public GetOrganisationNameQueryHandler(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        public async Task<GetOrganisationNameQueryResult> Handle(GetOrganisationNameQuery request, CancellationToken cancellationToken)
        {
            var account = await _accountsService.GetAccount(request.EncodedAccountId);

            return new GetOrganisationNameQueryResult
            {
                DasAccountName = account.DasAccountName
            };
        }
    }
}