using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserActions;

namespace SFA.DAS.DigitalCertificates.Api.Models.Users
{
    public class GetUserActionsResponse
    {
        [JsonPropertyName("useractions")]
        public IEnumerable<UserActionDetail> UserActions { get; set; } = new List<UserActionDetail>();

        public static implicit operator GetUserActionsResponse(GetUserActionsQueryResult source)
        {
            if (source == null) return null;

            return new GetUserActionsResponse
            {
                UserActions = source.UserActions?.Select(ua => new UserActionDetail
                {
                    Id = ua.Id,
                    UserId = ua.UserId,
                    ActionType = ua.ActionType,
                    ActionTime = ua.ActionTime,
                    ActionStatus = ua.ActionStatus,
                    FamilyName = ua.FamilyName,
                    GivenNames = ua.GivenNames,
                    Uln = ua.Uln,
                    CertificateId = ua.CertificateId,
                    CertificateType = ua.CertificateType,
                    CourseName = ua.CourseName,
                    ActionCode = ua.ActionCode,
                    AdminActions = ua.AdminActions?.Select(a => new AdminActionDetail
                    {
                        Username = a.Username,
                        ActionTime = a.ActionTime,
                        Action = a.Action
                    })
                })
            };
        }
    }

    public class UserActionDetail
    {
        public required long Id { get; set; }
        public required System.Guid UserId { get; set; }
        public required string ActionType { get; set; }
        public required System.DateTime ActionTime { get; set; }
        public required string ActionStatus { get; set; }
        public required string FamilyName { get; set; }
        public required string GivenNames { get; set; }
        public long? Uln { get; set; }
        public System.Guid? CertificateId { get; set; }
        public string CertificateType { get; set; }
        public string CourseName { get; set; }
        public string ActionCode { get; set; }
        public IEnumerable<AdminActionDetail> AdminActions { get; set; }
    }

    public class AdminActionDetail
    {
        public required string Username { get; set; }
        public required System.DateTime ActionTime { get; set; }
        public required string Action { get; set; }
    }
}
