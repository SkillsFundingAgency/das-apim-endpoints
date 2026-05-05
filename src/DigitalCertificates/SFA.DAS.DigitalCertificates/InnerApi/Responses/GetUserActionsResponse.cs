using System;
using System.Collections.Generic;

namespace SFA.DAS.DigitalCertificates.InnerApi.Responses
{
    public class GetUserActionsResponse
    {
        public List<UserAction> UserActions { get; set; } = new List<UserAction>();
    }

    public class UserAction
    {
        public long Id { get; set; }
        public required Guid UserId { get; set; }
        public required string ActionType { get; set; }
        public required string ActionCode { get; set; }
        public required DateTime ActionTime { get; set; }
        public required string ActionStatus { get; set; }
        public required string FamilyName { get; set; }
        public required string GivenNames { get; set; }
        public Guid? CertificateId { get; set; }
        public string CertificateType { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public List<AdminAction> AdminActions { get; set; } = new List<AdminAction>();
    }

    public class AdminAction
    {
        public required string Username { get; set; }
        public DateTime ActionTime { get; set; }
        public required string Action { get; set; }
    }
}
