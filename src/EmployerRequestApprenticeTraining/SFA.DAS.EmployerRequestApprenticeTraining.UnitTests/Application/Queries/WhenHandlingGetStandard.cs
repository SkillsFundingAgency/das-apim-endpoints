using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetStandard;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.UnitTests.Application.Queries
{
    public class WhenHandlingGetStandard
    {
        [Test, MoqAutoData]
        public async Task Then_Get_Standard_From_The_Api(
           StandardDetailResponse standardDetailResponse,
           GetStandardQuery query,
           [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
           GetStandardQueryHandler handler)
        {
            // Arrange
            var response = new ApiResponse<StandardDetailResponse>(standardDetailResponse, HttpStatusCode.OK, string.Empty);

            mockCoursesApiClient
                .Setup(client => client.GetWithResponseCode<StandardDetailResponse>(It.IsAny<GetStandardDetailsByIdRequest>()))
                .ReturnsAsync(response);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Standard.Should().BeEquivalentTo((Standard)standardDetailResponse, 
                options => options.ExcludingMissingMembers());
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Api_Call_Fails(
            GetStandardQuery query,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            GetStandardQueryHandler handler)
        {
            // Arrange
            mockCoursesApiClient
                .Setup(client => client.GetWithResponseCode<StandardDetailResponse>(It.IsAny<GetStandardDetailsByIdRequest>()))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.NotFound, "Not Found"));

            // Act & Assert
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);
            act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.NotFound);
        }
    }
}
