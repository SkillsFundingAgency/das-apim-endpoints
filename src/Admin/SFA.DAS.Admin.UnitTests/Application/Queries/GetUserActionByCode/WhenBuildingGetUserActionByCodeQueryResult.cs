using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Admin.Application.Queries.GetUserActionByCode;

namespace SFA.DAS.Admin.UnitTests.Application.Queries.GetUserActionByCode
{
    public class WhenBuildingGetUserActionByCodeQueryResult
    {
        [Test, AutoData]
        public void Then_Result_Properties_Are_Set_Correctly(
            long id,
            Guid userId,
            string actionType,
            DateTime actionTime,
            string actionStatus,
            long? uln,
            string familyName,
            string givenNames,
            Guid? certificateId,
            string certificateType,
            string courseName,
            string adminUsername,
            DateTime adminActionTime,
            string adminAction)
        {
            // Arrange & Act
            var result = new GetUserActionByCodeQueryResult
            {
                Id = id,
                UserId = userId,
                ActionType = actionType,
                ActionTime = actionTime,
                ActionStatus = actionStatus,
                Uln = uln,
                FamilyName = familyName,
                GivenNames = givenNames,
                CertificateId = certificateId,
                CertificateType = certificateType,
                CourseName = courseName,
                AdminActions = new List<GetUserActionByCodeQueryResult.AdminActionDetail>
                {
                    new GetUserActionByCodeQueryResult.AdminActionDetail
                    {
                        Username = adminUsername,
                        ActionTime = adminActionTime,
                        Action = adminAction
                    }
                }
            };

            // Assert
            result.Id.Should().Be(id);
            result.UserId.Should().Be(userId);
            result.ActionType.Should().Be(actionType);
            result.ActionTime.Should().Be(actionTime);
            result.ActionStatus.Should().Be(actionStatus);
            result.Uln.Should().Be(uln);
            result.FamilyName.Should().Be(familyName);
            result.GivenNames.Should().Be(givenNames);
            result.CertificateId.Should().Be(certificateId);
            result.CertificateType.Should().Be(certificateType);
            result.CourseName.Should().Be(courseName);
            result.AdminActions.Should().NotBeNull();
            result.AdminActions.Should().HaveCount(1);

            var admin = result.AdminActions.First();
            admin.Username.Should().Be(adminUsername);
            admin.ActionTime.Should().Be(adminActionTime);
            admin.Action.Should().Be(adminAction);
        }
    }
}
