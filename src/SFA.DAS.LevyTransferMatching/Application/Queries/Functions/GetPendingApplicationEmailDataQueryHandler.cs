using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Functions
{
    public class GetPendingApplicationEmailDataQueryHandler : IRequestHandler<GetPendingApplicationEmailDataQuery, GetPendingApplicationEmailDataQueryResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly IAccountsService _accountsService;


        public GetPendingApplicationEmailDataQueryHandler(ILevyTransferMatchingService levyTransferMatchingService, IAccountsService accountsService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _accountsService = accountsService;
        }

        public async Task<GetPendingApplicationEmailDataQueryResult> Handle(GetPendingApplicationEmailDataQuery request, CancellationToken cancellationToken)
        {
            var getEmployerAccountsResponse = await _levyTransferMatchingService.GetAccounts(new GetAccountsRequest());

            var emailDataList = await GetEmailDataFromAccountsResponse(getEmployerAccountsResponse);

            return new GetPendingApplicationEmailDataQueryResult
            {
                EmailDataList = emailDataList
            };
        }

        public async Task<List<GetPendingApplicationEmailDataQueryResult.EmailData>> GetEmailDataFromAccountsResponse(GetAccountsResponse getAccountsResponse)
        {
            var emailDataList = new List<GetPendingApplicationEmailDataQueryResult.EmailData>();
            foreach (var employerAccount in getAccountsResponse.EmployerAccounts)
            {
                List<TeamMember> users = await _accountsService.GetAccountUsers(employerAccount.Id);
                users = users.Where(x => (x.Role == "Owner" || x.Role == "Transactor") && x.CanReceiveNotifications).ToList();

                if (users.Any())
                {
                    var getApplicationsResponse = await _levyTransferMatchingService.GetApplications(new GetApplicationsRequest { AccountId = employerAccount.Id });
                    var pendingApplications = getApplicationsResponse.Applications.Where(x => x.Status == "Pending");

                    if (pendingApplications.Any())
                    {
                        foreach(var user in users)
                        {
                            emailDataList.Add(new GetPendingApplicationEmailDataQueryResult.EmailData
                            {
                                RecipientEmailAddress = user.Email,
                                EmployerName = employerAccount.Name,
                                NumberOfApplications = pendingApplications.Count(),
                                AccountId = employerAccount.Id
                            });
                        }
                    }
                }
            }

            return emailDataList;
        }
    }
}
