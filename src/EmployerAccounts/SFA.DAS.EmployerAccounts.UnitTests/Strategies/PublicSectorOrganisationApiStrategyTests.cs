using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Exceptions;
using SFA.DAS.EmployerAccounts.Strategies;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.PublicSectorOrganisations;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.PublicSectorOrganisation;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Strategies;

[TestFixture]
public class PublicSectorOrganisationApiStrategyTests
{
    [Test, MoqAutoData]
    public async Task Then_GetOrganisationDetails_Returns_And_IsMapped_To_Organisation(
        [Frozen] Mock<IPublicSectorOrganisationApiClient<PublicSectorOrganisationApiConfiguration>> mockApi,
        string identifier,
        OrganisationType organisationType,
        PublicSectorOrganisationApiStrategy strategy,
        PublicSectorOrganisation response)
    {
        var apiResponse = new ApiResponse<PublicSectorOrganisation>(response, System.Net.HttpStatusCode.OK, null);
        response.Active = true;

        mockApi
            .Setup(m => m.GetWithResponseCode<PublicSectorOrganisation>(
                It.Is<GetLatestDetailsForPublicSectorOrganisationRequest>(r => r.Identifier == identifier)))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await strategy.GetOrganisationDetails(identifier, organisationType);

        result.OrganisationDetail.Name.Should().Be(response.Name);
        result.OrganisationDetail.Type.Should().Be(OrganisationType.PublicSector);
        result.OrganisationDetail.Code.Should().Be(response.Id.ToString());
        result.OrganisationDetail.RegistrationDate.Should().BeNull();
        result.OrganisationDetail.Sector.Should().Be(response.OnsSector);
        result.OrganisationDetail.OrganisationStatus.Should().Be(OrganisationStatus.Active);
        result.OrganisationDetail.Address.Line1.Should().Be(response.AddressLine1);
        result.OrganisationDetail.Address.Line2.Should().Be(response.AddressLine2);
        result.OrganisationDetail.Address.Line3.Should().Be(response.AddressLine3);
        result.OrganisationDetail.Address.Line4.Should().Be(response.Town);
        result.OrganisationDetail.Address.Line5.Should().Be(response.Country);
        result.OrganisationDetail.Address.Postcode.Should().Be(response.PostCode);
    }

    [Test, MoqAutoData]
    public void Then_GetOrganisationDetails_ShouldThrowInvalidGetOrganisationException_WhenApiResponseIsBadRequest(
        [Frozen] Mock<IPublicSectorOrganisationApiClient<PublicSectorOrganisationApiConfiguration>> mockApi,
        string identifier,
        OrganisationType organisationType,
        PublicSectorOrganisationApiStrategy strategy,
        PublicSectorOrganisation response)
    {
        var errorMessage = "Error Message";
        var apiResponse =
            new ApiResponse<PublicSectorOrganisation>(null, System.Net.HttpStatusCode.BadRequest, errorMessage);

        mockApi
            .Setup(m => m.GetWithResponseCode<PublicSectorOrganisation>(
                It.Is<GetLatestDetailsForPublicSectorOrganisationRequest>(r => r.Identifier == identifier)))
            .ReturnsAsync(apiResponse);

        // Act
        Func<Task> act = async () => await strategy.GetOrganisationDetails(identifier, organisationType);

        act.Should().ThrowAsync<InvalidGetOrganisationException>();
    }

    [Test, MoqAutoData]
    public void Then_GetOrganisationDetails_ShouldOrganisationNotFoundException_WhenApiResponseIsBadRequest(
        [Frozen] Mock<IPublicSectorOrganisationApiClient<PublicSectorOrganisationApiConfiguration>> mockApi,
        string identifier,
        OrganisationType organisationType,
        PublicSectorOrganisationApiStrategy strategy,
        PublicSectorOrganisation response)
    {
        var errorMessage = "Error Message";
        var apiResponse =
            new ApiResponse<PublicSectorOrganisation>(null, System.Net.HttpStatusCode.NotFound, errorMessage);

        mockApi
            .Setup(m => m.GetWithResponseCode<PublicSectorOrganisation>(
                It.Is<GetLatestDetailsForPublicSectorOrganisationRequest>(r => r.Identifier == identifier)))
            .ReturnsAsync(apiResponse);

        // Act
        Func<Task> act = async () => await strategy.GetOrganisationDetails(identifier, organisationType);

        act.Should().ThrowAsync<OrganisationNotFoundException>();
    }
}
