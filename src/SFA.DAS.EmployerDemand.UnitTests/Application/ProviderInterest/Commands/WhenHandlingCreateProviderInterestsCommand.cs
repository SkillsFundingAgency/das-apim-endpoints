using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.ProviderInterest.Commands.CreateProviderInterests;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.UnitTests.Application.ProviderInterest.Commands
{
    public class WhenHandlingCreateProviderInterestsCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called(
            CreateProviderInterestsCommand command,
            PostCreateProviderInterestsResponse responseBody,
            [Frozen]Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> mockApiClient,
            CreateProviderInterestsCommandHandler handler)
        {
            //Arrange
            var apiResponse = new ApiResponse<PostCreateProviderInterestsResponse>(responseBody, HttpStatusCode.Created, null);
            mockApiClient
                .Setup(client => client.PostWithResponseCode<PostCreateProviderInterestsResponse>(
                    It.IsAny<PostCreateProviderInterestsRequest>()))
                .ReturnsAsync(apiResponse);

            //Act
            var response = await handler.Handle(command, CancellationToken.None);
            
            //Assert
            response.Should().Be(responseBody.Ukprn);
        }
    }
}