using System;
using System.Collections.Generic;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUserActions
{
    public class GetUserActionsQueryResult
    {
        public IEnumerable<UserActionDetail> UserActions { get; set; } = new List<UserActionDetail>();

        public static implicit operator GetUserActionsQueryResult(SFA.DAS.DigitalCertificates.InnerApi.Responses.GetUserActionsResponse source)
        {
            if (source == null) return null;

            var result = new GetUserActionsQueryResult
            {
                UserActions = source.UserActions?.ConvertAll(ua => new UserActionDetail
                {
                    Id = ua.Id,
                    UserId = ua.UserId,
                    ActionType = ua.ActionType,
                    ActionTime = ua.ActionTime,
                    ActionStatus = ua.ActionStatus ?? string.Empty,
                    FamilyName = ua.FamilyName ?? string.Empty,
                    GivenNames = ua.GivenNames ?? string.Empty,
                    CertificateId = ua.CertificateId,
                    CertificateType = ua.CertificateType,
                    CourseName = ua.CourseName,
                    ActionCode = ua.ActionCode,
                    AdminActions = ua.AdminActions?.ConvertAll(a => new AdminActionDetail
                    {
                        Username = a.Username,
                        ActionTime = a.ActionTime,
                        Action = a.Action
                    })
                }) ?? new List<UserActionDetail>()
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
