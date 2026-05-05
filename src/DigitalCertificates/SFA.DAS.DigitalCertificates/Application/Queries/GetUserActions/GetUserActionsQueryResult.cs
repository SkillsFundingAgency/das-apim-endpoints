using System;
using System.Collections.Generic;
// ActionStatus uses string now; no enum import required

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
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public string ActionType { get; set; } = string.Empty;
        public DateTime ActionTime { get; set; }
        public string ActionStatus { get; set; } = string.Empty;
        public string FamilyName { get; set; } = string.Empty;
        public string GivenNames { get; set; } = string.Empty;
        public Guid? CertificateId { get; set; }
        public string CertificateType { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string ActionCode { get; set; }
        public IEnumerable<AdminActionDetail> AdminActions { get; set; }
    }

    public class AdminActionDetail
    {
        public string Username { get; set; } = string.Empty;
        public DateTime ActionTime { get; set; }
        public string Action { get; set; } = string.Empty;
    }
}
