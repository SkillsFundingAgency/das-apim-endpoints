using System;
using System.Collections.Generic;
using SFA.DAS.Admin.Application.Queries.GetAllUserActivityByCode;

namespace SFA.DAS.Admin.Api.Models.UserActions
{
    public class GetAllUserActivityByCodeResponse
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
            public List<AdminAction> AdminActions { get; set; }
            public List<UserMatch> UserMatches { get; set; }
        }

        public class AdminAction
        {
            public string Username { get; set; }
            public DateTime ActionTime { get; set; }
            public string Action { get; set; }
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

        public static implicit operator GetAllUserActivityByCodeResponse(GetAllUserActivityByCodeQueryResult result)
        {
            if (result == null) return null;

            return new GetAllUserActivityByCodeResponse
            {
                UserId = result.UserId,
                GovUKIdentifier = result.GovUKIdentifier,
                EmailAddress = result.EmailAddress,
                PhoneNumber = result.PhoneNumber,
                CreatedAt = result.CreatedAt,
                LastLoginAt = result.LastLoginAt,
                IsLocked = result.IsLocked,
                LockedTime = result.LockedTime,
                UserActions = result.UserActions?.ConvertAll(a => new UserAction
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    ActionType = a.ActionType,
                    ActionCode = a.ActionCode,
                    ActionTime = a.ActionTime,
                    ActionStatus = a.ActionStatus,
                    Uln = a.Uln,
                    FamilyName = a.FamilyName,
                    GivenNames = a.GivenNames,
                    CertificateType = a.CertificateType,
                    CertificateId = a.CertificateId,
                    CourseName = a.CourseName,
                    AdminActions = a.AdminActions?.ConvertAll(ad => new AdminAction
                    {
                        Username = ad.Username,
                        ActionTime = ad.ActionTime,
                        Action = ad.Action
                    }),
                    UserMatches = a.UserMatches?.ConvertAll(um => new UserMatch
                    {
                        Id = um.Id,
                        Uln = um.Uln,
                        FamilyName = um.FamilyName,
                        DateOfBirth = um.DateOfBirth,
                        EventTime = um.EventTime,
                        CertificateType = um.CertificateType,
                        CourseCode = um.CourseCode,
                        CourseName = um.CourseName,
                        CourseLevel = um.CourseLevel,
                        DateAwarded = um.DateAwarded,
                        ProviderName = um.ProviderName,
                        Ukprn = um.Ukprn,
                        IsMatched = um.IsMatched,
                        IsFailed = um.IsFailed
                    })
                })
            };
        }
    }
}
