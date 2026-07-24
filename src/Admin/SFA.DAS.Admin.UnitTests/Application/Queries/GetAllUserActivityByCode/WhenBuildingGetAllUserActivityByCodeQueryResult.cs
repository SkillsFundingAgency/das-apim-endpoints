using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Admin.Application.Queries.GetAllUserActivityByCode;

namespace SFA.DAS.Admin.UnitTests.Application.Queries.GetAllUserActivityByCode
{
    public class WhenBuildingGetAllUserActivityByCodeQueryResult
    {
        [Test, AutoData]
        public void Then_Result_Properties_Are_Set_Correctly(
            Guid userId,
            string govUkIdentifier,
            string emailAddress,
            string phoneNumber,
            DateTime createdAt,
            DateTime? lastLoginAt,
            bool isLocked,
            DateTime lockedTime,
            long actionId,
            Guid actionUserId,
            string actionType,
            DateTime actionTime,
            string actionStatus,
            long? uln,
            string familyName,
            string givenNames,
            Guid? certificateId,
            string certificateType,
            string courseName,
            Guid matchId,
            long? matchUln,
            string matchFamilyName,
            DateTime matchDob,
            DateTime matchEventTime,
            string matchCertificateType,
            string matchCourseCode,
            string matchCourseName,
            string matchCourseLevel,
            int? matchDateAwarded,
            string matchProviderName,
            int? matchUkprn,
            bool matchIsMatched,
            bool matchIsFailed,
            string adminUsername,
            DateTime adminActionTime,
            string adminAction)
        {
            var result = new GetAllUserActivityByCodeQueryResult
            {
                UserId = userId,
                GovUKIdentifier = govUkIdentifier,
                EmailAddress = emailAddress,
                PhoneNumber = phoneNumber,
                CreatedAt = createdAt,
                LastLoginAt = lastLoginAt,
                IsLocked = isLocked,
                LockedTime = lockedTime,
                UserActions = new List<GetAllUserActivityByCodeQueryResult.UserAction>
                {
                    new GetAllUserActivityByCodeQueryResult.UserAction
                    {
                        Id = actionId,
                        UserId = actionUserId,
                        ActionType = actionType,
                        ActionTime = actionTime,
                        ActionStatus = actionStatus,
                        Uln = uln,
                        FamilyName = familyName,
                        GivenNames = givenNames,
                        CertificateId = certificateId,
                        CertificateType = certificateType,
                        CourseName = courseName,
                        UserMatches = new List<GetAllUserActivityByCodeQueryResult.UserMatch>
                        {
                            new GetAllUserActivityByCodeQueryResult.UserMatch
                            {
                                Id = matchId,
                                Uln = matchUln,
                                FamilyName = matchFamilyName,
                                DateOfBirth = matchDob,
                                EventTime = matchEventTime,
                                CertificateType = matchCertificateType,
                                CourseCode = matchCourseCode,
                                CourseName = matchCourseName,
                                CourseLevel = matchCourseLevel,
                                DateAwarded = matchDateAwarded,
                                ProviderName = matchProviderName,
                                Ukprn = matchUkprn,
                                IsMatched = matchIsMatched,
                                IsFailed = matchIsFailed
                            }
                        },
                        AdminActions = new List<GetAllUserActivityByCodeQueryResult.AdminAction>
                        {
                            new GetAllUserActivityByCodeQueryResult.AdminAction
                            {
                                Username = adminUsername,
                                ActionTime = adminActionTime,
                                Action = adminAction
                            }
                        }
                    }
                }
            };

            result.UserId.Should().Be(userId);
            result.GovUKIdentifier.Should().Be(govUkIdentifier);
            result.EmailAddress.Should().Be(emailAddress);
            result.PhoneNumber.Should().Be(phoneNumber);
            result.CreatedAt.Should().Be(createdAt);
            result.LastLoginAt.Should().Be(lastLoginAt);
            result.IsLocked.Should().Be(isLocked);
            result.LockedTime.Should().Be(lockedTime);

            result.UserActions.Should().NotBeNull();
            result.UserActions.Should().HaveCount(1);

            var action = result.UserActions.First();
            action.Id.Should().Be(actionId);
            action.UserId.Should().Be(actionUserId);
            action.ActionType.Should().Be(actionType);
            action.ActionTime.Should().Be(actionTime);
            action.ActionStatus.Should().Be(actionStatus);
            action.Uln.Should().Be(uln);
            action.FamilyName.Should().Be(familyName);
            action.GivenNames.Should().Be(givenNames);
            action.CertificateId.Should().Be(certificateId);
            action.CertificateType.Should().Be(certificateType);
            action.CourseName.Should().Be(courseName);

            action.UserMatches.Should().NotBeNull();
            action.UserMatches.Should().HaveCount(1);

            var match = action.UserMatches.First();
            match.Id.Should().Be(matchId);
            match.Uln.Should().Be(matchUln);
            match.FamilyName.Should().Be(matchFamilyName);
            match.DateOfBirth.Should().Be(matchDob);
            match.EventTime.Should().Be(matchEventTime);
            match.CertificateType.Should().Be(matchCertificateType);
            match.CourseCode.Should().Be(matchCourseCode);
            match.CourseName.Should().Be(matchCourseName);
            match.CourseLevel.Should().Be(matchCourseLevel);
            match.DateAwarded.Should().Be(matchDateAwarded);
            match.ProviderName.Should().Be(matchProviderName);
            match.Ukprn.Should().Be(matchUkprn);
            match.IsMatched.Should().Be(matchIsMatched);
            match.IsFailed.Should().Be(matchIsFailed);

            action.AdminActions.Should().NotBeNull();
            action.AdminActions.Should().HaveCount(1);

            var admin = action.AdminActions.First();
            admin.Username.Should().Be(adminUsername);
            admin.ActionTime.Should().Be(adminActionTime);
            admin.Action.Should().Be(adminAction);
        }
    }
}
