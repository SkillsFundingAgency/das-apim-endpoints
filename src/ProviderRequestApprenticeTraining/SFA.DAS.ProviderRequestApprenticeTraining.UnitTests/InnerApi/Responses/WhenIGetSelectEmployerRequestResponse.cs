using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Responses;
using System.Collections.Generic;
using System;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Responses
{
    [TestFixture]
    public class WhenIGetSelectEmployerRequestsResponse
    {
        [Test]
        public void GetSelectedEmployerRequestsResponse_PropertiesShouldBeSet()
        {
            // Arrange
            var response = new GetSelectEmployerRequestsResponse();

            // Act & Assert
            response.EmployerRequestId.Should().BeEmpty();
            response.StandardReference.Should().BeNull();
            response.StandardTitle.Should().BeNull();
            response.StandardLevel.Should().Be(0);
            response.SingleLocation.Should().BeNull();
            response.DateOfRequest.Should().Be(DateTime.MinValue);
            response.NumberOfApprentices.Should().Be(0);
            response.DayRelease.Should().BeFalse();
            response.BlockRelease.Should().BeFalse();
            response.AtApprenticesWorkplace.Should().BeFalse();
            response.IsNew.Should().BeFalse();
            response.IsContacted.Should().BeFalse();
            response.DateContacted.Should().BeNull();
            response.Locations.Should().BeNull();
    }
    }
}