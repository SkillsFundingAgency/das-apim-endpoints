using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.LevyTransferMatching.Models.Constants;
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
            
            var getApplicationsResponse = await _levyTransferMatchingService.GetApplications(new GetApplicationsRequest { ApplicationStatusFilter = PledgeStatus.Pending });
            var employerAccountIds = getApplicationsResponse.Applications.Select(x => x.SenderEmployerAccountId).Distinct();

            foreach (var employerAccountId in employerAccountIds)
            {
                var employerName = getApplicationsResponse.Applications.First(x => x.SenderEmployerAccountId == employerAccountId).SenderEmployerAccountName;
                var applicationCount = getApplicationsResponse.Applications.Count(x => x.SenderEmployerAccountId == employerAccountId);

                List<TeamMember> users = await _accountsService.GetAccountUsers(employerAccountId);
                users = users.Where(x => (x.Role == "Owner" || x.Role == "Transactor") && x.CanReceiveNotifications).ToList();

                foreach (var user in users)
                {
                    emailDataList.Add(new GetPendingApplicationEmailDataQueryResult.EmailData
                    {
                        RecipientEmailAddress = user.Email,
                        EmployerName = employerName,
                        NumberOfApplications = applicationCount,
                        AccountId = employerAccountId
                    });
                }
            }

            return emailDataList;
        }
    }
}
