using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetUserIdentityRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correct(Guid userId)
        {
            var request = new GetUserIdentityRequest(userId);

            request.GetUrl.Should().Be($"api/users/{userId}/identity");
        }
    }
}
