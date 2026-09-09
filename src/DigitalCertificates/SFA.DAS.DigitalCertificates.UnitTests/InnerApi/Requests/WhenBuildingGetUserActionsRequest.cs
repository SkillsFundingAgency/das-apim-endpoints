using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetUserActionsRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Includes_The_UserId(Guid userId)
        {
            // Arrange & Act
            var request = new GetUserActionsRequest(userId);

            // Assert
            request.GetUrl.Should().Be($"api/users/{userId}/user-actions");
        }
    }
}
