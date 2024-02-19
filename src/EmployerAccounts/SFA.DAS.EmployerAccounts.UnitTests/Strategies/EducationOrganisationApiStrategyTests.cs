using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Exceptions;
using SFA.DAS.EmployerAccounts.Strategies;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EducationalOrganisations;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EducationalOrganisation;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Strategies
{
    [TestFixture]
    public class EducationOrganisationApiStrategyTests
    {
        [Test, MoqAutoData]
        public async Task Then_GetOrganisationDetails_Returns_And_IsMapped_To_Organisation(
             [Frozen] Mock<IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration>> mockApi,
             string identifier,
             OrganisationType organisationType,
             EducationOrganisationApiStrategy strategy,
             GetLatestDetailsForEducationalOrgResponse response)
        {
            var apiResponse = new ApiResponse<GetLatestDetailsForEducationalOrgResponse>(response, System.Net.HttpStatusCode.OK, null);

            mockApi
                .Setup(m => m.GetWithResponseCode<GetLatestDetailsForEducationalOrgResponse>(
                    It.Is<GetLatestDetailsForEducationalOrgRequest>(r => r.Identifier == identifier)))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await strategy.GetOrganisationDetails(identifier, organisationType);

            result.OrganisationDetail.Name.Should().Be(response.EducationalOrganisation.Name);
            result.OrganisationDetail.Type.Should().Be(OrganisationType.EducationOrganisation);
            result.OrganisationDetail.SubType.Should().Be(OrganisationSubType.None);
            result.OrganisationDetail.Code.Should().Be(response.EducationalOrganisation.URN);
            result.OrganisationDetail.RegistrationDate.Should().BeNull();
            result.OrganisationDetail.Sector.Should().Be(response.EducationalOrganisation.EducationalType);

            result.OrganisationDetail.Address.Line1.Should().Be(response.EducationalOrganisation.AddressLine1);
            result.OrganisationDetail.Address.Line2.Should().Be(response.EducationalOrganisation.AddressLine2);
            result.OrganisationDetail.Address.Line3.Should().Be(response.EducationalOrganisation.AddressLine3);
            result.OrganisationDetail.Address.Line4.Should().Be(response.EducationalOrganisation.Town);
            result.OrganisationDetail.Address.Line5.Should().Be(response.EducationalOrganisation.County);
            result.OrganisationDetail.Address.Postcode.Should().Be(response.EducationalOrganisation.PostCode);
        }

        [Test, MoqAutoData]
        public void Then_GetOrganisationDetails_ShouldThrowInvalidGetOrganisationException_WhenApiResponseIsBadRequest(
             [Frozen] Mock<IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration>> mockApi,
             string identifier,
             OrganisationType organisationType,
             EducationOrganisationApiStrategy strategy,
             GetLatestDetailsForEducationalOrgResponse response)
        {
            var errorMessage = "Error Message";
            var apiResponse = new ApiResponse<GetLatestDetailsForEducationalOrgResponse>(null, System.Net.HttpStatusCode.BadRequest, errorMessage);

            mockApi
                .Setup(m => m.GetWithResponseCode<GetLatestDetailsForEducationalOrgResponse>(
                    It.Is<GetLatestDetailsForEducationalOrgRequest>(r => r.Identifier == identifier)))
                .ReturnsAsync(apiResponse);

            // Act
            Func<Task> act = async () => await strategy.GetOrganisationDetails(identifier, organisationType);

            act.Should().Throw<InvalidGetOrganisationException>();

        }

        [Test, MoqAutoData]
        public void Then_GetOrganisationDetails_ShouldThrowOrganisationNotFoundException_WhenApiResponseIs_NotFound(
           [Frozen] Mock<IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration>> mockApi,
           string identifier,
           OrganisationType organisationType,
           EducationOrganisationApiStrategy strategy,
           GetLatestDetailsForEducationalOrgResponse response)
        {
            var errorMessage = "Error Message";
            var apiResponse = new ApiResponse<GetLatestDetailsForEducationalOrgResponse>(null, System.Net.HttpStatusCode.NotFound, errorMessage);

            mockApi
                .Setup(m => m.GetWithResponseCode<GetLatestDetailsForEducationalOrgResponse>(
                    It.Is<GetLatestDetailsForEducationalOrgRequest>(r => r.Identifier == identifier)))
                .ReturnsAsync(apiResponse);

            // Act
            Func<Task> act = async () => await strategy.GetOrganisationDetails(identifier, organisationType);

            act.Should().Throw<OrganisationNotFoundException>();

        }
    }
}
