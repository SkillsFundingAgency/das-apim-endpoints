using System.Threading;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Forecasting.Application.Courses.Queries.GetStandardCoursesList;
using SFA.DAS.Forecasting.InnerApi.Requests;
using SFA.DAS.Forecasting.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Forecasting.UnitTests.Application.Courses.Queries
{
    public class WhenGettingStandards
    {
        [Test, MoqAutoData]
        public void Then_Gets_Standards_From_Courses_Api(
            GetStandardCoursesQuery query,
            GetStandardsListResponse coursesApiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            GetStandardCoursesQueryHandler handler)
        {
            //Arrange
            mockCoursesApiClient.Setup(client =>
                    client.Get<GetStandardsListResponse>(It.IsAny<GetAvailableToStartStandardsListRequest>()))
                .ReturnsAsync(coursesApiResponse);

            //Act
            var actual = handler.Handle(query, CancellationToken.None);

            //Assert
            actual.Result.Should().BeEquivalentTo(coursesApiResponse);
        }
    }
}