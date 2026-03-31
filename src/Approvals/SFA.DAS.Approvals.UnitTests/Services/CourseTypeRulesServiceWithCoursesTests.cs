using System;
using System.Threading.Tasks;
using SFA.DAS.Approvals.InnerApi.CoursesApi;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Requests;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Services
{
    [TestFixture]
    public class CourseTypeRulesServiceWithCourseTests
    {
        [Test, MoqAutoData]
        public async Task GetCourseTypeRulesAsync_WhenCourseAndLearnerAgeRulesFound_ReturnsResult(
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
            [Frozen] Mock<ICourseTypesApiClient> courseTypesApiClient,
            GetCourseLookupResponse courseResponse,
            GetLearnerAgeResponse learnerAgeResponse,
            string courseCode,
            CourseTypeRulesServiceWithCourses service)
        {
            // Arrange
            courseResponse.LearningType = "Apprenticeship";

            coursesApiClient
                .Setup(x => x.Get<GetCourseLookupResponse>(It.IsAny<GetCourseLookupByIdRequest>()))
                .ReturnsAsync(courseResponse);

            courseTypesApiClient
                .Setup(x => x.Get<GetLearnerAgeResponse>(It.IsAny<GetLearnerAgeRequest>()))
                .ReturnsAsync(learnerAgeResponse);

            // Act
            var result = await service.GetCourseTypeRulesAsync(courseCode);

            // Assert
            result.Should().NotBeNull();
            result.Course.Should().BeEquivalentTo(courseResponse);
            result.LearnerAgeRules.Should().BeEquivalentTo(learnerAgeResponse);

            coursesApiClient.Verify(x => x.Get<GetCourseLookupResponse>(It.Is<GetCourseLookupByIdRequest>(r => r.Id == courseCode)), Times.Once);
            courseTypesApiClient.Verify(x => x.Get<GetLearnerAgeResponse>(It.Is<GetLearnerAgeRequest>(r => r.GetUrl.Contains(courseResponse.CourseType))), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task GetCourseTypeRulesAsync_WhenCourseNotFound_ThrowsException(
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
            [Frozen] Mock<ICourseTypesApiClient> courseTypesApiClient,
            string courseCode,
            CourseTypeRulesServiceWithCourses service)
        {
            // Arrange
            coursesApiClient
                .Setup(x => x.Get<GetCourseLookupResponse>(It.IsAny<GetCourseLookupByIdRequest>()))
                .ReturnsAsync((GetCourseLookupResponse)null);

            // Act
            var act = () => service.GetCourseTypeRulesAsync(courseCode);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage($"Course not found for course ID {courseCode}");

            courseTypesApiClient.Verify(x => x.Get<GetLearnerAgeResponse>(It.IsAny<GetLearnerAgeRequest>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task GetCourseTypeRulesAsync_WhenLearnerAgeRulesNotFound_ThrowsException(
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
            [Frozen] Mock<ICourseTypesApiClient> courseTypesApiClient,
            GetCourseLookupResponse learningResponse,
            string courseCode,
            CourseTypeRulesServiceWithCourses service)
        {
            // Arrange
            learningResponse.LearningType = "Apprenticeship";

            coursesApiClient
                .Setup(x => x.Get<GetCourseLookupResponse>(It.IsAny<GetCourseLookupByIdRequest>()))
                .ReturnsAsync(learningResponse);

            courseTypesApiClient
                .Setup(x => x.Get<GetLearnerAgeResponse>(It.IsAny<GetLearnerAgeRequest>()))
                .ReturnsAsync((GetLearnerAgeResponse)null);

            // Act
            var act = () => service.GetCourseTypeRulesAsync(courseCode);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Learner age rules not found for apprenticeship type Apprenticeship");

            coursesApiClient.Verify(x => x.Get<GetCourseLookupResponse>(It.Is<GetCourseLookupByIdRequest>(r => r.Id == courseCode)), Times.Once);
            courseTypesApiClient.Verify(x => x.Get<GetLearnerAgeResponse>(It.Is<GetLearnerAgeRequest>(r => r.GetUrl.Contains(learningResponse.CourseType))), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task GetCourseTypeRulesAsync_WhenCoursesApiThrowsException_PropagatesException(
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
            [Frozen] Mock<ICourseTypesApiClient> courseTypesApiClient,
            string courseCode,
            CourseTypeRulesServiceWithCourses service)
        {
            // Arrange
            var expectedException = new Exception("Courses API error");
            coursesApiClient
                .Setup(x => x.Get<GetCourseLookupResponse>(It.IsAny<GetCourseLookupByIdRequest>()))
                .ThrowsAsync(expectedException);

            // Act
            var act = () => service.GetCourseTypeRulesAsync(courseCode);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .Where(e => e.Message == "Courses API error");

            courseTypesApiClient.Verify(x => x.Get<GetLearnerAgeResponse>(It.IsAny<GetLearnerAgeRequest>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task GetCourseTypeRulesAsync_WhenCourseTypesApiThrowsException_PropagatesException(
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
            [Frozen] Mock<ICourseTypesApiClient> courseTypesApiClient,
            GetCourseLookupResponse courseResponse,
            string courseCode,
            CourseTypeRulesServiceWithCourses service)
        {
            // Arrange
            courseResponse.LearningType = "Apprenticeship";
            var expectedException = new Exception("Course Types API error");

            coursesApiClient
                .Setup(x => x.Get<GetCourseLookupResponse>(It.IsAny<GetCourseLookupByIdRequest>()))
                .ReturnsAsync(courseResponse);

            courseTypesApiClient
                .Setup(x => x.Get<GetLearnerAgeResponse>(It.IsAny<GetLearnerAgeRequest>()))
                .ThrowsAsync(expectedException);

            // Act
            var act = () => service.GetCourseTypeRulesAsync(courseCode);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .Where(e => e.Message == "Course Types API error");

            coursesApiClient.Verify(x => x.Get<GetCourseLookupResponse>(It.Is<GetCourseLookupByIdRequest>(r => r.Id == courseCode)), Times.Once);
            courseTypesApiClient.Verify(x => x.Get<GetLearnerAgeResponse>(It.Is<GetLearnerAgeRequest>(r => r.GetUrl.Contains(courseResponse.CourseType))), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task GetRplRulesAsync_WhenStandardAndRplRulesFound_ReturnsResult(
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
            [Frozen] Mock<ICourseTypesApiClient> courseTypesApiClient,
            GetCourseLookupResponse courseResponse,
            GetRecognitionOfPriorLearningResponse rplResponse,
            string courseCode,
            CourseTypeRulesServiceWithCourses service)
        {
            // Arrange
            courseResponse.LearningType = "Apprenticeship";

            coursesApiClient
                .Setup(x => x.Get<GetCourseLookupResponse>(It.IsAny<GetCourseLookupByIdRequest>()))
                .ReturnsAsync(courseResponse);

            courseTypesApiClient
                .Setup(x => x.Get<GetRecognitionOfPriorLearningResponse>(It.IsAny<GetRecognitionOfPriorLearningRequest>()))
                .ReturnsAsync(rplResponse);

            // Act
            var result = await service.GetRplRulesAsync(courseCode);

            // Assert
            result.Should().NotBeNull();
            result.Course.Should().BeEquivalentTo(courseResponse);
            result.RplRules.Should().BeEquivalentTo(rplResponse);

            coursesApiClient.Verify(x => x.Get<GetCourseLookupResponse>(It.Is<GetCourseLookupByIdRequest>(r => r.Id == courseCode)), Times.Once);
            courseTypesApiClient.Verify(x => x.Get<GetRecognitionOfPriorLearningResponse>(It.Is<GetRecognitionOfPriorLearningRequest>(r => r.GetUrl.Contains(courseResponse.CourseType))), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task GetRplRulesAsync_WhenCourseNotFound_ThrowsException(
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
            [Frozen] Mock<ICourseTypesApiClient> courseTypesApiClient,
            string courseCode,
            CourseTypeRulesServiceWithCourses service)
        {
            // Arrange
            coursesApiClient
                .Setup(x => x.Get<GetCourseLookupResponse>(It.IsAny<GetCourseLookupByIdRequest>()))
                .ReturnsAsync((GetCourseLookupResponse)null);

            // Act
            var act = () => service.GetRplRulesAsync(courseCode);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage($"Course not found for course ID {courseCode}");

            courseTypesApiClient.Verify(x => x.Get<GetRecognitionOfPriorLearningResponse>(It.IsAny<GetRecognitionOfPriorLearningRequest>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task GetRplRulesAsync_WhenRplRulesNotFound_ThrowsException(
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
            [Frozen] Mock<ICourseTypesApiClient> courseTypesApiClient,
            GetCourseLookupResponse courseResponse,
            string courseCode,
            CourseTypeRulesServiceWithCourses service)
        {
            // Arrange
            courseResponse.LearningType = "Apprenticeship";

            coursesApiClient
                .Setup(x => x.Get<GetCourseLookupResponse>(It.IsAny<GetCourseLookupByIdRequest>()))
                .ReturnsAsync(courseResponse);

            courseTypesApiClient
                .Setup(x => x.Get<GetRecognitionOfPriorLearningResponse>(It.IsAny<GetRecognitionOfPriorLearningRequest>()))
                .ReturnsAsync((GetRecognitionOfPriorLearningResponse)null);

            // Act
            var act = () => service.GetRplRulesAsync(courseCode);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("RPL rules not found for apprenticeship type Apprenticeship");

            coursesApiClient.Verify(x => x.Get<GetCourseLookupResponse>(It.Is<GetCourseLookupByIdRequest>(r => r.Id == courseCode)), Times.Once);
            courseTypesApiClient.Verify(x => x.Get<GetRecognitionOfPriorLearningResponse>(It.Is<GetRecognitionOfPriorLearningRequest>(r => r.GetUrl.Contains(courseResponse.CourseType))), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task GetRplRulesAsync_WhenCoursesApiThrowsException_PropagatesException(
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
            [Frozen] Mock<ICourseTypesApiClient> courseTypesApiClient,
            string courseCode,
            CourseTypeRulesServiceWithCourses service)
        {
            // Arrange
            var expectedException = new Exception("Courses API error");
            coursesApiClient
                .Setup(x => x.Get<GetCourseLookupResponse>(It.IsAny<GetCourseLookupByIdRequest>()))
                .ThrowsAsync(expectedException);

            // Act
            var act = () => service.GetRplRulesAsync(courseCode);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .Where(e => e.Message == "Courses API error");

            courseTypesApiClient.Verify(x => x.Get<GetRecognitionOfPriorLearningResponse>(It.IsAny<GetRecognitionOfPriorLearningRequest>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task GetRplRulesAsync_WhenCourseTypesApiThrowsException_PropagatesException(
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
            [Frozen] Mock<ICourseTypesApiClient> courseTypesApiClient,
            GetCourseLookupResponse courseResponse,
            string courseCode,
            CourseTypeRulesServiceWithCourses service)
        {
            // Arrange
            courseResponse.LearningType = "Apprenticeship";
            var expectedException = new Exception("Course Types API error");

            coursesApiClient
                .Setup(x => x.Get<GetCourseLookupResponse>(It.IsAny<GetCourseLookupByIdRequest>()))
                .ReturnsAsync(courseResponse);

            courseTypesApiClient
                .Setup(x => x.Get<GetRecognitionOfPriorLearningResponse>(It.IsAny<GetRecognitionOfPriorLearningRequest>()))
                .ThrowsAsync(expectedException);

            // Act
            var act = () => service.GetRplRulesAsync(courseCode);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .Where(e => e.Message == "Course Types API error");

            coursesApiClient.Verify(x => x.Get<GetCourseLookupResponse>(It.Is<GetCourseLookupByIdRequest>(r => r.Id == courseCode)), Times.Once);
            courseTypesApiClient.Verify(x => x.Get<GetRecognitionOfPriorLearningResponse>(It.Is<GetRecognitionOfPriorLearningRequest>(r => r.GetUrl.Contains(courseResponse.CourseType))), Times.Once);
        }
    }
}