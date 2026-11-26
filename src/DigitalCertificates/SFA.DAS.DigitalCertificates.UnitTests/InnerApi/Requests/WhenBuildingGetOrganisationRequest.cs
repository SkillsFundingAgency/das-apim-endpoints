using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetOrganisationRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built(Guid id)
        {
            // Arrange & Act
            var request = new GetOrganisationRequest(id);

            // Assert
            request.GetUrl.Should().Be($"/api/v1/organisations/organisation/{id}");
        }

        [Test, AutoData]
        public void Then_The_OrganisationId_Is_Set(Guid id)
        {
            // Arrange & Act
            var request = new GetOrganisationRequest(id);

            // Assert
            request.OrganisationId.Should().Be(id);
        }
    }
}
