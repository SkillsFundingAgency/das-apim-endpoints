using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerProfileUser;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerRequest;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerRequestApprenticeTraining.UnitTests.Application.Queries
{
    public class WhenHandlingGetEmployerRequest
    {
        [Test, MoqAutoData]
        public async Task Then_Get_EmployerRequest_From_The_Api(
           EmployerRequest employerRequest,
           GetEmployerRequestQuery query,
           [Frozen] Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> mockRequestApprenticeTrainingClient,
           GetEmployerRequestQueryHandler handler)
        {
            // Arrange
            var response = new ApiResponse<EmployerRequest>(employerRequest, HttpStatusCode.OK, string.Empty);
            
            mockRequestApprenticeTrainingClient
                .Setup(client => client.GetWithResponseCode<EmployerRequest>(It.IsAny<GetEmployerRequestRequest>()))
                .ReturnsAsync(response);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            // Assert
            actual.EmployerRequest.Should().BeEquivalentTo(employerRequest, options => options.ExcludingMissingMembers());
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Api_Call_Fails(
            GetEmployerRequestQuery query,
            [Frozen] Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> mockRequestApprenticeTrainingClient,
            GetEmployerRequestQueryHandler handler)
        {
            // Arrange
            mockRequestApprenticeTrainingClient
                .Setup(client => client.GetWithResponseCode<EmployerRequest>(It.IsAny<GetEmployerRequestRequest>()))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.NotFound, "Not Found"));

            // Act & Assert
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);
            act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.NotFound);
        }
    }
}
