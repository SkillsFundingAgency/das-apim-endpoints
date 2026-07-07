using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;
using SFA.DAS.Apim.Shared.Extensions;
using System.Net;
using SFA.DAS.DigitalCertificates.Contracts.Client;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System;

using CreateAdminActionCommand = SFA.DAS.DigitalCertificates.Contracts.ApiResponses.CreateAdminActionCommand;
using PostUsersAdminactionsApiRequest = SFA.DAS.DigitalCertificates.Contracts.ApiRequests.PostUsersAdminactionsApiRequest;

namespace SFA.DAS.Admin.Application.Commands.CheckUserActionByCode
{
    public class CheckUserActionByCodeCommandHandler : IRequestHandler<CheckUserActionByCodeCommand, CheckUserActionByCodeResult>
    {
        private readonly IMediator _mediator;
        private readonly IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> _digitalCertificatesApiClient;

        public CheckUserActionByCodeCommandHandler(IMediator mediator, IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> digitalCertificatesApiClient)
        {
            _mediator = mediator;
            _digitalCertificatesApiClient = digitalCertificatesApiClient;
        }

        public async Task<CheckUserActionByCodeResult> Handle(CheckUserActionByCodeCommand request, CancellationToken cancellationToken)
        {
            var apiResponse = await _digitalCertificatesApiClient.GetWithResponseCode<UserActionDetail>(new GetUsersUseractionsByCodeApiRequest(request.Code));

            if (apiResponse?.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            apiResponse?.EnsureSuccessStatusCode();

            var response = apiResponse?.Body;

            if (response == null) return null;

            var shouldLog = false;

            if (response.AdminActions == null || response.AdminActions.Count == 0)
            {
                shouldLog = true;
            }
            else
            {
                var mostRecent = response.AdminActions.OrderByDescending(a => a.ActionTime).FirstOrDefault();

                if (mostRecent == null || !string.Equals(mostRecent.Username, request.Username, StringComparison.OrdinalIgnoreCase) ||
                    mostRecent.Action != AdminActionType.Viewed)
                {
                    shouldLog = true;
                }
            }

            if (shouldLog)
            {
                var create = new CreateAdminActionCommand
                {
                    Username = request.Username,
                    Action = AdminActionType.Viewed,
                    UserActionId = response.Id
                };

                var postResponse = await _digitalCertificatesApiClient.PostWithResponseCode<CreateAdminActionCommand>(
                    new PostUsersAdminactionsApiRequest(create));

                postResponse?.EnsureSuccessStatusCode();
            }

            var result = new CheckUserActionByCodeResult
            {
                Id = response.Id,
                UserId = response.UserId,
                ActionType = response.ActionType.ToString(),
                ActionTime = response.ActionTime,
                ActionStatus = response.ActionStatus.ToString(),
                Uln = response.Uln,
                FamilyName = response.FamilyName,
                GivenNames = response.GivenNames,
                CertificateId = response.CertificateId,
                CertificateType = response.CertificateType.ToString(),
                CourseName = response.CourseName,
                AdminActions = response.AdminActions?.Select(a => new CheckUserActionByCodeResult.AdminActionDetail
                {
                    Username = a.Username,
                    ActionTime = a.ActionTime,
                    Action = a.Action.ToString()
                }).ToList()
            };

            return result;
        }
    }
}
