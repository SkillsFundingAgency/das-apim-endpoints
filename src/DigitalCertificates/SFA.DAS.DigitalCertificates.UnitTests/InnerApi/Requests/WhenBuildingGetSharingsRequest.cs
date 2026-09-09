using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetSharingsRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built(Guid userId, Guid certificateId, int limit)
        {
            // Arrange & Act
            var request = new GetSharingsRequest(userId, certificateId, limit);

            // Assert
            request.GetUrl.Should().Be($"api/users/{userId}/sharings?certificateId={certificateId}&limit={limit}");
        }
    }
}
