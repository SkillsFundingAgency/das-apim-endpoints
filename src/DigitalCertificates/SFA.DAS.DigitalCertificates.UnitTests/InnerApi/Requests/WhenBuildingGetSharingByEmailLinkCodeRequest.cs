using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetSharingByEmailLinkCodeRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built(Guid code)
        {
            var request = new GetSharingByEmailLinkCodeRequest(code);

            request.GetUrl.Should().Be($"api/sharingemail/emaillinkcode/{code}");
        }
    }
}
