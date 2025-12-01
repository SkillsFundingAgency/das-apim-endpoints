using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetCertificateSharingDetailsRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built(Guid userId, Guid certificateId, int limit)
        {
            var request = new GetCertificateSharingDetailsRequest(userId, certificateId, limit);

            request.GetUrl.Should().Be($"api/sharing?user={userId}&certificateid={certificateId}&limit={limit}");
        }
    }
}
