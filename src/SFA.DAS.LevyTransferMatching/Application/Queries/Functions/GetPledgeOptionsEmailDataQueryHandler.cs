using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.LevyTransferMatching.Models.Constants;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var emailDataList = await GetEmailDataFromAccountsResponse();

            return new GetPledgeOptionsEmailDataQueryResult
            {
                EmailDataList = emailDataList
            };
        }

        public async Task<List<GetPledgeOptionsEmailDataQueryResult.EmailData>> GetEmailDataFromAccountsResponse()
        {
            var emailDataList = new List<GetPledgeOptionsEmailDataQueryResult.EmailData>();

            var getPledgesResponse = await _levyTransferMatchingService.GetPledges(new GetPledgesRequest(pledgeStatusFilter: PledgeStatus.Active));
            var employerAccountIds = getPledgesResponse.Pledges.Select(x => x.AccountId).Distinct();

            foreach (var employerAccountId in employerAccountIds)
            {
                var employerName = getPledgesResponse.Pledges.First(x => x.AccountId == employerAccountId).DasAccountName;

                List<TeamMember> users = await _accountsService.GetAccountUsers(employerAccountId);
                users = users.Where(x => (x.Role == "Owner" || x.Role == "Transactor") && x.CanReceiveNotifications).ToList();

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
