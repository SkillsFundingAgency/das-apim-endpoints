using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetFrameworkCertificateRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built(Guid id)
        {
            // Arrange & Act
            var request = new GetFrameworkCertificateRequest(id);

            // Assert
            request.GetUrl.Should().Be($"api/v1/learnerdetails/framework-learner/{id}?allLogs=false");
        }
    }
}
