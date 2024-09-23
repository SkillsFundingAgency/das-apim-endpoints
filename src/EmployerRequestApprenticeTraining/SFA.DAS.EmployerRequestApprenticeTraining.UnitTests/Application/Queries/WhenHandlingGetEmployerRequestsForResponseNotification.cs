using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerRequestsForResponseNotification;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.UnitTests.Application.Queries
{
    public class WhenHandlingGetEmployerRequestsForResponseNotifications
    {
        [Test, MoqAutoData]
        public async Task Then_Get_EmployerRequestsForNotifications_From_The_Api(
           List<EmployerRequestForResponseNotification> employerRequests,
           GetEmployerRequestsForResponseNotificationQuery query,
           [Frozen] Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> mockRequestApprenticeTrainingClient,
           GetEmployerRequestsForResponseNotificationQueryHandler handler)
        {
            // Arrange
            var response = new ApiResponse<List<EmployerRequestForResponseNotification>>(employerRequests, HttpStatusCode.OK, string.Empty);

            mockRequestApprenticeTrainingClient
                .Setup(client => client.GetWithResponseCode<List<EmployerRequestForResponseNotification>>(It.IsAny<GetEmployerRequestsForResponseNotificationRequest>()))
                .ReturnsAsync(response);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.EmployerRequests.Should().BeEquivalentTo(employerRequests.Select(s => (SharedOuterApi.Models.RequestApprenticeTraining.EmployerRequestForResponseNotification)s).ToList(), 
                options => options.ExcludingMissingMembers());
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Api_Call_Fails(
            GetEmployerRequestsForResponseNotificationQuery query,
            [Frozen] Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> mockRequestApprenticeTrainingClient,
            GetEmployerRequestsForResponseNotificationQueryHandler handler)
        {
            // Arrange
            mockRequestApprenticeTrainingClient
                .Setup(client => client.GetWithResponseCode<List<EmployerRequestForResponseNotification>>(It.IsAny<GetEmployerRequestsForResponseNotificationRequest>()))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.NotFound, "Not Found"));

            // Act & Assert
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);
            act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.NotFound);
        }
    }
}
