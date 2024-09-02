using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Responses;
using System.Collections.Generic;
using System;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Responses
{
    [TestFixture]
    public class WhenIGetProviderResponseConfirmationResponse
    {
        [Test]
        public void GetProviderResponseConfirmationResponse_PropertiesShouldBeSet()
        {
            // Arrange
            var response = new GetProviderResponseConfirmationResponse();

            // Act & Assert
            response.Email.Should().BeNull();
            response.Phone.Should().BeNull();
            response.Website.Should().BeNull();
            response.Ukprn.Should().Be(0);
            response.EmployerRequests.Should().BeNullOrEmpty();
    }
    }
}