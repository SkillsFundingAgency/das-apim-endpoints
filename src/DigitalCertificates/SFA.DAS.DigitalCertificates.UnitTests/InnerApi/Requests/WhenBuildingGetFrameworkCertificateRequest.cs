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
        public void Then_The_GetUrl_Includes_AllLogs_When_True(Guid id)
        {
            var request = new GetFrameworkCertificateRequest(id, true);

            request.GetUrl.Should().Be($"api/v1/learnerdetails/framework-learner/{id}?allLogs=true");
        }

        [Test, AutoData]
        public void Then_The_GetUrl_Excludes_AllLogs_When_Default(Guid id)
        {
            var request = new GetFrameworkCertificateRequest(id);

            request.GetUrl.Should().Be($"api/v1/learnerdetails/framework-learner/{id}");
        }
    }
}
