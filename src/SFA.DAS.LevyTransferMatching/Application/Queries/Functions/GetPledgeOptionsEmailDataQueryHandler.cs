using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.LevyTransferMatching.Models.Constants;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Functions
{
    public class GetPledgeOptionsEmailDataQueryHandler : IRequestHandler<GetPledgeOptionsEmailDataQuery, GetPledgeOptionsEmailDataQueryResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly IAccountsService _accountsService;

        public GetPledgeOptionsEmailDataQueryHandler(ILevyTransferMatchingService levyTransferMatchingService, IAccountsService accountsService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _accountsService = accountsService;
        }

        public async Task<GetPledgeOptionsEmailDataQueryResult> Handle(GetPledgeOptionsEmailDataQuery request, CancellationToken cancellationToken)
        {
            var getEmployerAccountsResponse = await _levyTransferMatchingService.GetAccounts(new GetAccountsRequest());

            var emailDataList = await GetEmailDataFromAccountsResponse(getEmployerAccountsResponse);

            return new GetPledgeOptionsEmailDataQueryResult
            {
                EmailDataList = emailDataList
            };
        }

        public async Task<List<GetPledgeOptionsEmailDataQueryResult.EmailData>> GetEmailDataFromAccountsResponse(GetAccountsResponse getAccountsResponse)
        {
            var emailDataList = new List<GetPledgeOptionsEmailDataQueryResult.EmailData>();

            var getPledgesResponse = await _levyTransferMatchingService.GetPledges(new GetPledgesRequest { PledgeStatusFilter = PledgeStatus.Active });
            var employerAccountIds = getPledgesResponse.Pledges.Select(x => x.AccountId).Distinct();

            foreach (var employerAccountId in employerAccountIds)
            {
                var employerName = getPledgesResponse.Pledges.First(x => x.AccountId == employerAccountId).DasAccountName;

                List<TeamMember> users = await _accountsService.GetAccountUsers(employerAccountId);
                users = users.Where(x => (x.Role == "Owner" || x.Role == "Transactor") && x.CanReceiveNotifications).ToList();
                users.Add(new TeamMember());
                foreach (var user in users)
                {
                    emailDataList.Add(new GetPledgeOptionsEmailDataQueryResult.EmailData
                    {
                        RecipientEmailAddress = user.Email,
                        EmployerName = employerName,
                        AccountId = employerAccountId,
                        FinancialYearStart = DateTime.Now.Year.ToString(),
                        FinancialYearEnd = DateTime.Now.AddYears(1).Year.ToString()
                    });
                }
            }

            return emailDataList;
        }
    }
}
