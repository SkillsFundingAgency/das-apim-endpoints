using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Exceptions;
using SFA.DAS.EmployerAccounts.Strategies;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Strategies
{
    [TestFixture]
    public class CharitiesApiStrategyTests
    {

        [Test, MoqAutoData]
        public async Task Then_GetOrganisationDetails_Returns(
             [Frozen] Mock<ICharitiesApiClient<CharitiesApiConfiguration>> mockApi,
             int registrationNumber,
             CharitiesApiStrategy strategy,
             GetCharityResponse response)
        {
            var identifier = registrationNumber.ToString();
            var apiResponse = new ApiResponse<GetCharityResponse>(response, System.Net.HttpStatusCode.OK, null);

            mockApi
                .Setup(m => m.GetWithResponseCode<GetCharityResponse>(
                    It.Is<GetCharityRequest>(r => r.RegistrationNumber == registrationNumber)))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await strategy.GetOrganisationDetails(identifier, OrganisationType.Charity);

            result.OrganisationDetail.Should().NotBeNull();
            result.OrganisationDetail.Name.Should().Be(response.Name);
        }


        [Test, MoqAutoData]
        public void Then_GetOrganisationDetails_ShouldThrowInvalidGetOrganisationException_WhenApiResponseIsBadRequest(
             [Frozen] Mock<ICharitiesApiClient<CharitiesApiConfiguration>> mockApi,
             string identifier,
             CharitiesApiStrategy strategy,
             Organisation response)
        {
            var errorMessage = "Error Message";

            var apiResponse = new ApiResponse<Organisation>(response, System.Net.HttpStatusCode.BadRequest, errorMessage);

            mockApi
                .Setup(m => m.GetWithResponseCode<Organisation>(
                    It.Is<GetLatestDetailsRequest>(r => r.Identifier == identifier)))
                .ReturnsAsync(apiResponse);

            // Act
            Func<Task> act = async () => await strategy.GetOrganisationDetails(identifier, OrganisationType.Charity);

            act.Should().ThrowAsync<InvalidGetOrganisationException>();
        }


        [Test, MoqAutoData]
        public void Then_GetOrganisationDetails_ShouldThrowOrganisationNotFoundException_WhenApiResponse_Is_NotFound(
           [Frozen] Mock<ICharitiesApiClient<CharitiesApiConfiguration>> mockApi,
           string identifier,
           CharitiesApiStrategy strategy,
           Organisation response)
        {
            var errorMessage = "Error Message";

            var apiResponse = new ApiResponse<Organisation>(response, System.Net.HttpStatusCode.NotFound, errorMessage);

            mockApi
                .Setup(m => m.GetWithResponseCode<Organisation>(
                    It.Is<GetLatestDetailsRequest>(r => r.Identifier == identifier)))
                .ReturnsAsync(apiResponse);

            // Act
            Func<Task> act = async () => await strategy.GetOrganisationDetails(identifier, OrganisationType.Charity);

            act.Should().ThrowAsync<OrganisationNotFoundException>();
        }
    }
}
