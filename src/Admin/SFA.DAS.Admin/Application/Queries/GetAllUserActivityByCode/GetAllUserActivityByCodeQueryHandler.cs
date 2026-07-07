using MediatR;
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.DigitalCertificates.Contracts.Client;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;

namespace SFA.DAS.Admin.Application.Queries.GetAllUserActivityByCode
{
    public class GetAllUserActivityByCodeQueryHandler : IRequestHandler<GetAllUserActivityByCodeQuery, GetAllUserActivityByCodeQueryResult>
    {
        private readonly IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> _digitalCertificatesApiClient;

        public GetAllUserActivityByCodeQueryHandler(IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> digitalCertificatesApiClient)
        {
            _digitalCertificatesApiClient = digitalCertificatesApiClient;
        }

        public async Task<GetAllUserActivityByCodeQueryResult> Handle(GetAllUserActivityByCodeQuery request, CancellationToken cancellationToken)
        {
            var codeResponse = await _digitalCertificatesApiClient.GetWithResponseCode<GetUserActionByCodeQueryResult>(new GetUsersUseractionsByCodeApiRequest(request.Code));

            if (codeResponse?.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            codeResponse?.EnsureSuccessStatusCode();

            var codeBody = codeResponse?.Body;

            if (codeBody == null) return null;

            var userId = codeBody.UserId;

            var userActionsTask = _digitalCertificatesApiClient.GetWithResponseCode<GetUserActionsQueryResult>(new GetUsersByUserIdActionsApiRequest(userId));
            var userDetailsTask = _digitalCertificatesApiClient.GetWithResponseCode<GetUserByIdQueryResult>(new GetUsersIdByUserIdApiRequest(userId));

            await Task.WhenAll(userActionsTask, userDetailsTask);

            var userActionsResponse = userActionsTask.Result;
            var userDetailsResponse = userDetailsTask.Result;

            userActionsResponse?.EnsureSuccessStatusCode();
            userDetailsResponse?.EnsureSuccessStatusCode();

            var userActionsBody = userActionsResponse?.Body;
            var userDetailsBody = userDetailsResponse?.Body;

            if (userDetailsBody == null) return null;

            var result = new GetAllUserActivityByCodeQueryResult
            {
                UserId = userDetailsBody.UserId,
                GovUKIdentifier = userDetailsBody.GovUkIdentifier,
                EmailAddress = userDetailsBody.EmailAddress,
                PhoneNumber = userDetailsBody.PhoneNumber,
                CreatedAt = userDetailsBody.CreatedAt,
                LastLoginAt = userDetailsBody.LastLoginAt,
                IsLocked = userDetailsBody.IsLocked,
                UserActions = new List<GetAllUserActivityByCodeQueryResult.UserAction>()
            };

            var userMatches = userDetailsBody.UserMatches?
                .Where(m => m != null)
                .OrderByDescending(m => m.EventTime)
                .ToList() ?? new List<UserMatchDetail>();

            var userActions = userActionsBody?.UserActions?
                .OrderByDescending(ua => ua.ActionTime)
                .Select(ua =>
                {
                    var action = new GetAllUserActivityByCodeQueryResult.UserAction
                    {
                        Id = ua.Id,
                        UserId = ua.UserId,
                        ActionType = ua.ActionType.ToString(),
                        ActionCode = ua.ActionCode,
                        ActionTime = ua.ActionTime,
                        ActionStatus = ua.ActionStatus.ToString(),
                        Uln = ua.Uln,
                        FamilyName = ua.FamilyName,
                        GivenNames = ua.GivenNames,
                        CertificateId = ua.CertificateId,
                        CertificateType = ua.CertificateType.ToString(),
                        CourseName = ua.CourseName,
                        AdminActions = ua.AdminActions?
                            .Where(a => a != null)
                            .Select(a => new GetAllUserActivityByCodeQueryResult.AdminAction
                            {
                                Username = a.Username,
                                ActionTime = a.ActionTime,
                                Action = a.Action.ToString()
                            }).ToList()
                    };

                    if (ua.ActionType == ActionType.NotMatched)
                    {
                        action.UserMatches = userMatches
                            .Where(m => m.EventTime <= ua.ActionTime)
                            .Take(2)
                            .Select(m => new GetAllUserActivityByCodeQueryResult.UserMatch
                            {
                                Id = m.Id,
                                Uln = m.Uln,
                                FamilyName = m.FamilyName,
                                DateOfBirth = m.DateOfBirth,
                                EventTime = m.EventTime,
                                CertificateType = m.CertificateType.ToString(),
                                CourseCode = m.CourseCode,
                                CourseName = m.CourseName,
                                CourseLevel = m.CourseLevel,
                                DateAwarded = m.DateAwarded,
                                ProviderName = m.ProviderName,
                                Ukprn = m.Ukprn,
                                IsMatched = m.IsMatched,
                                IsFailed = m.IsFailed
                            }).ToList();
                    }

                    return action;
                })
                .ToList() ?? new List<GetAllUserActivityByCodeQueryResult.UserAction>();

            result.UserActions = userActions;

            if (result.IsLocked)
            {
                result.LockedTime = userActions
                    .Where(a => a.ActionType == ActionType.NotMatched.ToString())
                    .Max(a => (DateTime?)a.ActionTime);
            }

            return result;
        }
    }
}
