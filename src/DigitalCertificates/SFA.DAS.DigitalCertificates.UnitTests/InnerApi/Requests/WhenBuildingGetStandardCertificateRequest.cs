using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetStandardCertificateRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Includes_IncludeLogs_When_True(Guid id)
        {
            // Arrange & Act
            var request = new GetStandardCertificateRequest(id, true);

            // Assert
            request.GetUrl.Should().Be($"api/v1/certificates/{id}?includeLogs=true");
        }

        [Test, AutoData]
        public void Then_The_GetUrl_Excludes_IncludeLogs_When_Default(Guid id)
        {
            // Arrange & Act
            var request = new GetStandardCertificateRequest(id);

            // Assert
            request.GetUrl.Should().Be($"api/v1/certificates/{id}");
        }
    }
}
