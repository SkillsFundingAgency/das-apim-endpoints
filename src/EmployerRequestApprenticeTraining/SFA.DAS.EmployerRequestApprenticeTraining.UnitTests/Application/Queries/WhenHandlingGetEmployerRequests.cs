using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerProfileUser;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerRequests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using EmployerRequest = SFA.DAS.SharedOuterApi.InnerApi.Responses.RequestApprenticeTraining.EmployerRequest;

namespace SFA.DAS.EmployerRequestApprenticeTraining.UnitTests.Application.Queries
{
    public class WhenHandlingGetEmployerRequests
    {
        [Test, MoqAutoData]
        public async Task Then_Get_EmployerRequests_From_The_Api(
           List<EmployerRequest> employerRequests,
           GetEmployerRequestsQuery query,
           [Frozen] Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> mockRequestApprenticeTrainingClient,
           GetEmployerRequestsQueryHandler handler)
        {
            // Arrange
            var response = new ApiResponse<List<EmployerRequest>>(employerRequests, HttpStatusCode.OK, string.Empty);

            mockRequestApprenticeTrainingClient
                .Setup(client => client.GetWithResponseCode<List<EmployerRequest>>(It.IsAny<GetEmployerRequestsRequest>()))
                .ReturnsAsync(response);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.EmployerRequests.Should().BeEquivalentTo(employerRequests.Select(s => (SharedOuterApi.Models.RequestApprenticeTraining.EmployerRequest)s).ToList(), 
                options => options.ExcludingMissingMembers());
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Api_Call_Fails(
            GetEmployerRequestsQuery query,
            [Frozen] Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> mockRequestApprenticeTrainingClient,
            GetEmployerRequestsQueryHandler handler)
        {
            // Arrange
            mockRequestApprenticeTrainingClient
                .Setup(client => client.GetWithResponseCode<List<EmployerRequest>>(It.IsAny<GetEmployerRequestsRequest>()))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.NotFound, "Not Found"));

            // Act & Assert
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);
            act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.NotFound);
        }
    }
}
