using System;
using System.Collections.Generic;

namespace SFA.DAS.Admin.Application.Queries.GetAllUserActivityByCode
{
    public class GetAllUserActivityByCodeQueryResult
    {
        public Guid UserId { get; set; }
        public string GovUKIdentifier { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? LockedTime { get; set; }
        public List<UserAction> UserActions { get; set; }

        public class UserAction
        {
            public long Id { get; set; }
            public Guid UserId { get; set; }
            public string ActionType { get; set; }
            public string ActionCode { get; set; }
            public DateTime ActionTime { get; set; }
            public string ActionStatus { get; set; }
            public long? Uln { get; set; }
            public string FamilyName { get; set; }
            public string GivenNames { get; set; }
            public Guid? CertificateId { get; set; }
            public string CertificateType { get; set; }
            public string CourseName { get; set; }
            public List<UserMatch> UserMatches { get; set; }
            public List<AdminAction> AdminActions { get; set; }
        }

        public class UserMatch
        {
            public Guid Id { get; set; }
            public long? Uln { get; set; }
            public string FamilyName { get; set; }
            public DateTime DateOfBirth { get; set; }
            public DateTime EventTime { get; set; }
            public string CertificateType { get; set; }
            public string CourseCode { get; set; }
            public string CourseName { get; set; }
            public string CourseLevel { get; set; }
            public int? DateAwarded { get; set; }
            public string ProviderName { get; set; }
            public int? Ukprn { get; set; }
            public bool IsMatched { get; set; }
            public bool IsFailed { get; set; }
        }

        public class AdminAction
        {
            public string Username { get; set; }
            public DateTime ActionTime { get; set; }
            public string Action { get; set; }
        }
    }
}
