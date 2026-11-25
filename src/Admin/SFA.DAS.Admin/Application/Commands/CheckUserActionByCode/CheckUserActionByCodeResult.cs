
using System;
using System.Collections.Generic;

namespace SFA.DAS.Admin.Application.Commands.CheckUserActionByCode
{
    public class CheckUserActionByCodeResult
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public string ActionType { get; set; }
        public DateTime ActionTime { get; set; }
        public string ActionStatus { get; set; }
        public long? Uln { get; set; }
        public string FamilyName { get; set; }
        public string GivenNames { get; set; }
        public Guid? CertificateId { get; set; }
        public string CertificateType { get; set; }
        public string CourseName { get; set; }
        public List<AdminActionDetail> AdminActions { get; set; }

        public class AdminActionDetail
        {
            public string Username { get; set; }
            public DateTime ActionTime { get; set; }
            public string Action { get; set; }
        }
    }
}
