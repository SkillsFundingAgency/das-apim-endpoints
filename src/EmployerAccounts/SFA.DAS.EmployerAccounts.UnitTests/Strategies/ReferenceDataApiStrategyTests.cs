using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Exceptions;
using SFA.DAS.EmployerAccounts.Strategies;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Strategies
{
    [TestFixture]
    public class ReferenceDataApiStrategyTests
    {

        [Test, MoqAutoData]
        public async Task Then_GetOrganisationDetails_Returns(
             [Frozen] Mock<IReferenceDataApiClient<ReferenceDataApiConfiguration>> mockApi,
             string identifier,
             OrganisationType organisationType,
             ReferenceDataApiStrategy strategy,
             Organisation response)
        {
            var apiResponse = new ApiResponse<Organisation>(response, System.Net.HttpStatusCode.OK, null);

            mockApi
                .Setup(m => m.GetWithResponseCode<Organisation>(
                    It.Is<GetLatestDetailsRequest>(r => r.Identifier == identifier)))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await strategy.GetOrganisationDetails(identifier, organisationType);

            result.OrganisationDetail.Should().NotBeNull();
            result.OrganisationDetail.Should().BeEquivalentTo(response);
        }


        [Test, MoqAutoData]
        public void Then_GetOrganisationDetails_ShouldThrowInvalidGetOrganisationException_WhenApiResponseIsBadRequest(
             [Frozen] Mock<IReferenceDataApiClient<ReferenceDataApiConfiguration>> mockApi,
             string identifier,
             OrganisationType organisationType,
             ReferenceDataApiStrategy strategy,
             Organisation response)
        {
            var errorMessage = "Error Message";

            var apiResponse = new ApiResponse<Organisation>(response, System.Net.HttpStatusCode.BadRequest, errorMessage);

            mockApi
                .Setup(m => m.GetWithResponseCode<Organisation>(
                    It.Is<GetLatestDetailsRequest>(r => r.Identifier == identifier)))
                .ReturnsAsync(apiResponse);

            // Act
            Func<Task> act = async () => await strategy.GetOrganisationDetails(identifier, organisationType);

            act.Should().Throw<InvalidGetOrganisationException>();
        }


        [Test, MoqAutoData]
        public void Then_GetOrganisationDetails_ShouldThrowOrganisationNotFoundException_WhenApiResponse_Is_NotFound(
           [Frozen] Mock<IReferenceDataApiClient<ReferenceDataApiConfiguration>> mockApi,
           string identifier,
           OrganisationType organisationType,
           ReferenceDataApiStrategy strategy,
           Organisation response)
        {
            var errorMessage = "Error Message";

            var apiResponse = new ApiResponse<Organisation>(response, System.Net.HttpStatusCode.NotFound, errorMessage);

            mockApi
                .Setup(m => m.GetWithResponseCode<Organisation>(
                    It.Is<GetLatestDetailsRequest>(r => r.Identifier == identifier)))
                .ReturnsAsync(apiResponse);

            // Act
            Func<Task> act = async () => await strategy.GetOrganisationDetails(identifier, organisationType);

            act.Should().Throw<OrganisationNotFoundException>();
        }
    }
}
