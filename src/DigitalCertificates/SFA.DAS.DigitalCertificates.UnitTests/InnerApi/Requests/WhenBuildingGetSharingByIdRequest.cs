using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetSharingByIdRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built_With_Limit(Guid sharingId, int limit)
        {
            // Arrange & Act
            var request = new GetSharingByIdRequest(sharingId, limit);

            // Assert
            request.GetUrl.Should().Be($"api/sharing/{sharingId}?limit={limit}");
        }

        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built_With_Null_Limit(Guid sharingId)
        {
            // Arrange & Act
            var request = new GetSharingByIdRequest(sharingId, null);

            // Assert
            request.GetUrl.Should().Be($"api/sharing/{sharingId}?limit={""}");
        }
    }
}
