using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLatestStandards;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.UnitTests.Application.Queries
{
    public class WhenHandlingGetLatestStandards
    {
        [Test, MoqAutoData]
        public async Task Then_GetsStandards_From_The_Api(
           GetStandardsListResponse standardDetailListResponse,
           GetLatestStandardsQuery query,
           [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
           GetLatestStandardsQueryHandler handler)
        {
            // Arrange
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(It.IsAny<GetAvailableToStartStandardsListRequest>()))
                .ReturnsAsync(standardDetailListResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Standards.Should().BeEquivalentTo(standardDetailListResponse.Standards,
                options => options.ExcludingMissingMembers());

        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Api_Call_Fails(
            GetLatestStandardsQuery query,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            GetLatestStandardsQueryHandler handler)
        {
            // Arrange
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(It.IsAny<GetAvailableToStartStandardsListRequest>()))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.NotFound, "Not Found"));

            // Act & Assert
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);
            act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.NotFound);
        }
    }
}
