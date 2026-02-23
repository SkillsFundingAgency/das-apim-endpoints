using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Queries.GetActiveStandards;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class GetActiveStandardsQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_Maps_Standards_Correctly(
            GetActiveStandardsQuery query)
        {
            // Arrange
            var mockCourseService = new Mock<ICourseService>();

            var apiResponse = new GetStandardsListResponse
            {
                Standards = new List<GetStandardResponse>
                {
                    new GetStandardResponse
                    {
                        StandardUId = "ST0001_1.0",
                        Title = "Software Developer"
                    },
                    new GetStandardResponse
                    {
                        StandardUId = "ST0002_1.0",
                        Title = "Data Analyst"
                    }
                }
            };

            mockCourseService
                .Setup(x => x.GetActiveStandards<GetStandardsListResponse>(string.Empty))
                .ReturnsAsync(apiResponse);

            var handler = new GetActiveStandardsQueryHandler(mockCourseService.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Courses.Should().HaveCount(2);

            var courses = result.Courses.ToList();

            courses[0].StandardUId.Should().Be("ST0001_1.0");
            courses[0].Title.Should().Be("Software Developer");

            courses[1].StandardUId.Should().Be("ST0002_1.0");
            courses[1].Title.Should().Be("Data Analyst");

            mockCourseService.Verify(x =>
                x.GetActiveStandards<GetStandardsListResponse>(string.Empty),
                Times.Once);
        }
    }
}
