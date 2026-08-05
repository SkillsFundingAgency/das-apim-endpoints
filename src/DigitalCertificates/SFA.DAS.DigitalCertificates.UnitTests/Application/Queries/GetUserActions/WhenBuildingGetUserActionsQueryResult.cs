using System;
using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserActions;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetUserActions
{
    public class WhenBuildingGetUserActionsQueryResult
    {
        [Test, AutoData]
        public void Then_Result_Properties_Are_Set_Correctly(
            long id,
            Guid userId,
            string actionType,
            DateTime actionTime,
            string actionStatus,
            string familyName,
            string givenNames,
            Guid? certificateId,
            string certificateType,
            string courseName,
            string actionCode,
            string adminUsername,
            DateTime adminActionTime,
            string adminAction)
        {
            // Arrange & Act
            var result = new GetUserActionsQueryResult
            {
                UserActions = new List<UserActionDetail>
                {
                    new UserActionDetail
                    {
                        Id = id,
                        UserId = userId,
                        ActionType = actionType,
                        ActionTime = actionTime,
                        ActionStatus = actionStatus,
                        FamilyName = familyName,
                        GivenNames = givenNames,
                        CertificateId = certificateId,
                        CertificateType = certificateType,
                        CourseName = courseName,
                        ActionCode = actionCode,
                        AdminActions = new List<AdminActionDetail>
                        {
                            new AdminActionDetail
                            {
                                Username = adminUsername,
                                ActionTime = adminActionTime,
                                Action = adminAction
                            }
                        }
                    }
                }
            };

            // Assert
            result.UserActions.Should().HaveCount(1);

            var ua = System.Linq.Enumerable.First(result.UserActions);
            ua.Id.Should().Be(id);
            ua.UserId.Should().Be(userId);
            ua.ActionType.Should().Be(actionType);
            ua.ActionTime.Should().Be(actionTime);
            ua.ActionStatus.Should().Be(actionStatus);
            ua.FamilyName.Should().Be(familyName);
            ua.GivenNames.Should().Be(givenNames);
            ua.CertificateId.Should().Be(certificateId);
            ua.CertificateType.Should().Be(certificateType);
            ua.CourseName.Should().Be(courseName);
            ua.ActionCode.Should().Be(actionCode);
            ua.AdminActions.Should().NotBeNull();
            ua.AdminActions.Should().HaveCount(1);
            var admin = System.Linq.Enumerable.First(ua.AdminActions);
            admin.Username.Should().Be(adminUsername);
            admin.ActionTime.Should().Be(adminActionTime);
            admin.Action.Should().Be(adminAction);
        }
    }
}
