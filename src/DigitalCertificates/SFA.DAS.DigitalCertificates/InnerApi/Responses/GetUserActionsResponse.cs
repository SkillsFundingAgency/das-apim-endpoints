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
        public Guid UserId { get; set; }
        public string ActionType { get; set; }
        public string ActionCode { get; set; }
        public DateTime ActionTime { get; set; }
        public string ActionStatus { get; set; }
        public string FamilyName { get; set; }
        public string GivenNames { get; set; }
        public Guid? CertificateId { get; set; }
        public string CertificateType { get; set; }
        public string CourseName { get; set; }
        public List<AdminAction> AdminActions { get; set; } = new List<AdminAction>();
    }

    public class AdminAction
    {
        public string Username { get; set; }
        public DateTime ActionTime { get; set; }
        public string Action { get; set; }
    }
}
