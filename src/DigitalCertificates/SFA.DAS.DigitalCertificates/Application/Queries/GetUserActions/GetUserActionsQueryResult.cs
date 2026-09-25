using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUserActions
{
    public class GetUserActionsQueryResult
    {
        public IEnumerable<UserActionDetail> UserActions { get; set; } = new List<UserActionDetail>();

        public static implicit operator GetUserActionsQueryResult(GetUserActionsResponse source)
        {
            if (source == null) return null;

            var result = new GetUserActionsQueryResult
            {
                UserActions = source.UserActions?.Select(ua => new UserActionDetail
                {
                    Id = ua.Id,
                    UserId = ua.UserId,
                    ActionType = ua.ActionType.ToString(),
                    ActionTime = ua.ActionTime,
                    ActionStatus = ua.ActionStatus.ToString(),
                    FamilyName = ua.FamilyName ?? string.Empty,
                    GivenNames = ua.GivenNames ?? string.Empty,
                    Uln = ua.Uln,
                    CertificateId = ua.CertificateId,
                    CertificateType = ua.CertificateType.ToString(),
                    CourseName = ua.CourseName,
                    ActionCode = ua.ActionCode,
                    AdminActions = ua.AdminActions?.Select(a => new AdminActionDetail
                    {
                        Username = a.Username,
                        ActionTime = a.ActionTime,
                        Action = a.Action.ToString()
                    }).ToList()
                }).ToList() ?? new List<UserActionDetail>()
            };

            return result;
        }
    }

    public class UserActionDetail
    {
        public required long Id { get; set; }
        public required Guid UserId { get; set; }
        public required string ActionType { get; set; }
        public required DateTime ActionTime { get; set; }
        public required string ActionStatus { get; set; }
        public required string FamilyName { get; set; }
        public required string GivenNames { get; set; }
        public long? Uln { get; set; }
        public Guid? CertificateId { get; set; }
        public string CertificateType { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public required string ActionCode { get; set; }
        public IEnumerable<AdminActionDetail> AdminActions { get; set; }
    }

    public class AdminActionDetail
    {
        public required string Username { get; set; }
        public required DateTime ActionTime { get; set; }
        public required string Action { get; set; }
    }
}
