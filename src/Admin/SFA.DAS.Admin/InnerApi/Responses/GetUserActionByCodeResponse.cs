using System;
using System.Collections.Generic;

namespace SFA.DAS.Admin.InnerApi.Responses
{
    public class GetUserActionByCodeResponse
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public string ActionType { get; set; } = string.Empty;
        public DateTime ActionTime { get; set; }
        public string ActionStatus { get; set; } = string.Empty;
        public long? Uln { get; set; }
        public string FamilyName { get; set; } = string.Empty;
        public string GivenNames { get; set; } = string.Empty;
        public Guid? CertificateId { get; set; }
        public string CertificateType { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public List<AdminAction> AdminActions { get; set; } = new List<AdminAction>();
    }

    public class AdminAction
    {
        public string Username { get; set; } = string.Empty;
        public DateTime ActionTime { get; set; }
        public string Action { get; set; } = string.Empty;
    }
}
