using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using static SFA.DAS.DigitalCertificates.InnerApi.Requests.PostUpdateUserIdentityRequest;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPostUpdateUserIdentityRequest
    {
        [Test, AutoData]
        public void Then_The_PostUrl_Is_Correctly_Built(
            Guid userId,
            PostUpdateUserIdentityRequestData data)
        {
            var request = new PostUpdateUserIdentityRequest(data, userId);

            request.PostUrl.Should().Be($"api/users/{userId}/identity");
            request.Data.Should().Be(data);
        }
    }
}