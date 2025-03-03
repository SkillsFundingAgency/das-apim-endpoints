using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetActiveEmployerRequest;
using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.UnitTests.Application.Queries
{
    public class WhenHandlingGetActiveEmployerRequest
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_EmployerRequest_From_Api_By_AccountId_And_StandardReference(
           EmployerRequest employerRequest,
           GetActiveEmployerRequestQuery query,
           [Frozen] Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> mockRequestApprenticeTrainingClient,
           GetActiveEmployerRequestQueryHandler handler)
        {
            // Arrange
            query.AccountId = 123;
            query.StandardReference = "ST0123";

            var response = new ApiResponse<EmployerRequest>(employerRequest, HttpStatusCode.OK, string.Empty);

            mockRequestApprenticeTrainingClient
                .Setup(client => client.GetWithResponseCode<EmployerRequest>(It.Is<GetActiveEmployerRequestRequest>(r =>
                    r.AccountId == query.AccountId.Value && r.StandardReference == query.StandardReference)))
                .ReturnsAsync(response);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.EmployerRequest.Should().BeEquivalentTo(employerRequest, options => options.ExcludingMissingMembers());
        }

        [Test, MoqAutoData]
        public async Task Then_Returns_Null_When_EmployerRequest_Not_Found(
           GetActiveEmployerRequestQuery query,
           [Frozen] Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> mockRequestApprenticeTrainingClient,
           GetActiveEmployerRequestQueryHandler handler)
        {
            // Arrange
            query.AccountId = 123;
            query.StandardReference = "ST0123";

            var notFoundResponse = new ApiResponse<EmployerRequest>(null, HttpStatusCode.NotFound, string.Empty);

            mockRequestApprenticeTrainingClient
                .Setup(client => client.GetWithResponseCode<EmployerRequest>(It.Is<GetActiveEmployerRequestRequest>(r =>
                    r.AccountId == query.AccountId.Value && r.StandardReference == query.StandardReference)))
                .ReturnsAsync(notFoundResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.EmployerRequest.Should().BeNull();
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Api_Call_Fails(
            GetActiveEmployerRequestQuery query,
            [Frozen] Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> mockRequestApprenticeTrainingClient,
            GetActiveEmployerRequestQueryHandler handler)
        {
            // Arrange
            mockRequestApprenticeTrainingClient
                .Setup(client => client.GetWithResponseCode<EmployerRequest>(It.IsAny<GetActiveEmployerRequestRequest>()))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.BadRequest, "Bad Request"));

            // Act & Assert
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);
            act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.BadRequest);
        }
    }
}
