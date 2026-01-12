using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetFrameworkLearnerRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built(Guid frameworkLearnerId)
        {
            // Arrange & Act
            var request = new GetFrameworkLearnerRequest(frameworkLearnerId);

            // Assert
            request.GetUrl.Should().Be($"api/v1/learnerdetails/framework-learner/{frameworkLearnerId}?allLogs=false");
        }
    }
}
