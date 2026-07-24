using System;
using System.Collections.Generic;
using System.Linq;
using GetUserActionByCodeQueryResponse=SFA.DAS.DigitalCertificates.Contracts.ApiResponses.GetUserActionByCodeResponse;

namespace SFA.DAS.Admin.Application.Queries.GetUserActionByCode
{
    public class GetUserActionByCodeQueryResult
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
        public int? StandardCode { get; set; }
        public List<AdminActionDetail> AdminActions { get; set; }

        public class AdminActionDetail
        {
            public string Username { get; set; }
            public DateTime ActionTime { get; set; }
            public string Action { get; set; }
        }

        public static implicit operator GetUserActionByCodeQueryResult(GetUserActionByCodeQueryResponse response)
        {
            if (response == null) return null;
            return new GetUserActionByCodeQueryResult
            {
                Id = response.Id,
                UserId = response.UserId,
                ActionType = response.ActionType.ToString(),
                ActionTime = response.ActionTime,
                ActionStatus = response.ActionStatus.ToString(),
                Uln = response.Uln,
                FamilyName = response.FamilyName,
                GivenNames = response.GivenNames,
                CertificateId = response.CertificateId,
                CertificateType = response.CertificateType.ToString(),
                CourseName = response.CourseName,
                AdminActions = response.AdminActions?.Select(a => new AdminActionDetail
                {
                    Username = a.Username,
                    ActionTime = a.ActionTime,
                    Action = a.Action.ToString()
                }).ToList()
            };
        }
    }
}
