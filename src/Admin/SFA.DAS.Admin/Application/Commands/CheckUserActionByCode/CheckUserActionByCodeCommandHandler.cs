using SFA.DAS.Admin.InnerApi.Requests;
using SFA.DAS.Admin.InnerApi.Responses;
using System.Net;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Extensions;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System;
using SFA.DAS.Admin.Enums;

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
            var apiResponse = await _digitalCertificatesApiClient.GetWithResponseCode<GetUserActionByCodeResponse>(new GetUserActionByCodeRequest(request.Code));

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
                    !IsAdminAction(mostRecent.Action, AdminActionType.Viewed))
                {
                    shouldLog = true;
                }
            }

            if (shouldLog)
            {
                var postRequest = new PostAdminActionRequest(request.Username, AdminActionType.Viewed.ToString(), response.Id);
                            await _digitalCertificatesApiClient
                 .PostWithResponseCode<PostAdminActionRequestData, object>(postRequest);
            }

            var result = new CheckUserActionByCodeResult
            {
                Id = response.Id,
                UserId = response.UserId,
                ActionType = response.ActionType,
                ActionTime = response.ActionTime,
                ActionStatus = response.ActionStatus,
                Uln = response.Uln,
                FamilyName = response.FamilyName,
                GivenNames = response.GivenNames,
                CertificateId = response.CertificateId,
                CertificateType = response.CertificateType,
                CourseName = response.CourseName,
                AdminActions = response.AdminActions?.ConvertAll(a => new CheckUserActionByCodeResult.AdminActionDetail
                {
                    Username = a.Username,
                    ActionTime = a.ActionTime,
                    Action = a.Action
                })
            };

            return result;
        }

        private static bool IsAdminAction(string value, AdminActionType expected)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            return Enum.TryParse<AdminActionType>(value, true, out var parsed) && parsed == expected;
        }
    }
}
