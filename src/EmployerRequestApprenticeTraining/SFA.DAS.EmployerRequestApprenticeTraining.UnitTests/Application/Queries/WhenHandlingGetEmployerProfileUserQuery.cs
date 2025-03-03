using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerProfileUser;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerRequestApprenticeTraining.UnitTests.Application.Queries
{
    public class WhenHandlingGetEmployerProfileUserQuery
    {
        private Mock<IEmployerProfilesApiClient<EmployerProfilesApiConfiguration>> _mockEmployerProfilesApiClient;
        private GetEmployerProfileUserQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockEmployerProfilesApiClient = new Mock<IEmployerProfilesApiClient<EmployerProfilesApiConfiguration>>();
            _handler = new GetEmployerProfileUserQueryHandler(_mockEmployerProfilesApiClient.Object);
        }

        [Test, MoqAutoData]
        public async Task Then_The_EmployerProfileUser_Is_Returned(
            GetEmployerProfileUserQuery query,
            EmployerProfileUsersApiResponse apiResponse)
        {
            // Arrange
            var response = new ApiResponse<EmployerProfileUsersApiResponse>(apiResponse, HttpStatusCode.OK, string.Empty);
            _mockEmployerProfilesApiClient
                .Setup(client => client.GetWithResponseCode<EmployerProfileUsersApiResponse>(It.IsAny<GetEmployerUserAccountRequest>()))
                .ReturnsAsync(response);

            // Act
            var actual = await _handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Should().NotBeNull();
            actual.Id.Should().Be(apiResponse.Id);
            actual.Email.Should().Be(apiResponse.Email);
            actual.FirstName.Should().Be(apiResponse.FirstName);
            actual.LastName.Should().Be(apiResponse.LastName);
            actual.DisplayName.Should().Be(apiResponse.DisplayName);
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Api_Call_Fails(
            GetEmployerProfileUserQuery query)
        {
            // Arrange
            _mockEmployerProfilesApiClient
                .Setup(client => client.GetWithResponseCode<EmployerProfileUsersApiResponse>(It.IsAny<GetEmployerUserAccountRequest>()))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.NotFound, "Not Found"));

            // Act & Assert
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);
            act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.NotFound);
        }
    }
}
