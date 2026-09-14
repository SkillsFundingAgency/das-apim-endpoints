using System;
using System.Collections.Generic;
using SFA.DAS.Admin.Application.Queries.GetUserActionByCode;

namespace SFA.DAS.Admin.Api.Models.UserActions
{
    public class GetUserActionByCodeResponse
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
        public List<AdminAction> AdminActions { get; set; }

        public class AdminAction
        {
            public string Username { get; set; }
            public DateTime ActionTime { get; set; }
            public string Action { get; set; }
        }

        public static implicit operator GetUserActionByCodeResponse(GetUserActionByCodeQueryResult result)
        {
            if (result == null) return null;
            return new GetUserActionByCodeResponse
            {
                Id = result.Id,
                UserId = result.UserId,
                ActionType = result.ActionType,
                ActionTime = result.ActionTime,
                ActionStatus = result.ActionStatus,
                Uln = result.Uln,
                FamilyName = result.FamilyName,
                GivenNames = result.GivenNames,
                CertificateId = result.CertificateId,
                CertificateType = result.CertificateType,
                CourseName = result.CourseName,
                StandardCode = result.StandardCode,
                AdminActions = result.AdminActions?.ConvertAll(a => new AdminAction { Username = a.Username, ActionTime = a.ActionTime, Action = a.Action })
            };
        }
    }
}
