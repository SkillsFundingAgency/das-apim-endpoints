using System;
using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Responses
{
    public class WhenBuildingGetUserActionsResponse
    {
        [Test, AutoData]
        public void Then_UserAction_Items_Can_Be_Constructed()
        {
            // Arrange
            var adminAction = new AdminAction
            {
                Username = "admin.user",
                ActionTime = DateTime.UtcNow,
                Action = "Viewed"
            };

            var userAction = new UserAction
            {
                Id = 41L,
                UserId = Guid.NewGuid(),
                ActionType = "Reprint",
                ActionCode = "R",
                ActionTime = DateTime.UtcNow,
                ActionStatus = "New",
                FamilyName = "Smith",
                GivenNames = "John",
                CertificateId = Guid.NewGuid(),
                CertificateType = "Standard",
                CourseName = "Course",
                AdminActions = new List<AdminAction> { adminAction }
            };

            // Act
            var response = new GetUserActionsResponse
            {
                UserActions = new List<UserAction> { userAction }
            };

            // Assert
            response.UserActions.Should().ContainSingle();
            response.UserActions[0].AdminActions.Should().ContainSingle();
            response.UserActions[0].ActionType.Should().Be("Reprint");
            response.UserActions[0].FamilyName.Should().Be("Smith");
        }
    }
}
