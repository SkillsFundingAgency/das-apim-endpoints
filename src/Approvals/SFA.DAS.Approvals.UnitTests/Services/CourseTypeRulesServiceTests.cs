using System;
using System.Threading.Tasks;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Requests;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Responses;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Services
{
    [TestFixture]
    public class CourseTypeRulesServiceTests
    {
        [Test, MoqAutoData]
        public async Task GetCourseTypeRulesAsync_WhenStandardAndLearnerAgeRulesFound_ReturnsResult(
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
            [Frozen] Mock<ICourseTypesApiClient> courseTypesApiClient,
            GetStandardsListItem courseResponse,
            GetLearnerAgeResponse learnerAgeResponse,
            string courseCode,
            CourseTypeRulesService service)
        {
            // Arrange
            coursesApiClient
                .Setup(x => x.Get<GetStandardsListItem>(It.IsAny<GetStandardDetailsByIdRequest>()))
                .ReturnsAsync(courseResponse);

            courseTypesApiClient
                .Setup(x => x.Get<GetLearnerAgeResponse>(It.IsAny<GetLearnerAgeRequest>()))
                .ReturnsAsync(learnerAgeResponse);

            // Act
            var result = await service.GetCourseTypeRulesAsync(courseCode);

            // Assert
            result.Should().NotBeNull();
            result.Course.Should().BeEquivalentTo(courseResponse, o => o.ExcludingMissingMembers().Excluding(c=>c.LarsCode));
            result.Course.LarsCode.Should().Be(courseResponse.LarsCode.ToString());
            result.Course.LearningType.Should().Be(courseResponse.ApprenticeshipType);
            result.LearnerAgeRules.Should().BeEquivalentTo(learnerAgeResponse);

            coursesApiClient.Verify(x => x.Get<GetStandardsListItem>(It.Is<GetStandardDetailsByIdRequest>(r => r.Id == courseCode)), Times.Once);
            courseTypesApiClient.Verify(x => x.Get<GetLearnerAgeResponse>(It.Is<GetLearnerAgeRequest>(r => r.GetUrl.Contains(courseResponse.ApprenticeshipType))), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task GetCourseTypeRulesAsync_WhenStandardNotFound_ThrowsException(
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
            [Frozen] Mock<ICourseTypesApiClient> courseTypesApiClient,
            string courseCode,
            CourseTypeRulesService service)
        {

            // Arrange
            coursesApiClient
                .Setup(x => x.Get<GetStandardsListItem>(It.IsAny<GetStandardDetailsByIdRequest>()))
                .ReturnsAsync((GetStandardsListItem)null);

            // Act
            var act = () => service.GetCourseTypeRulesAsync(courseCode);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage($"Standard not found for course ID {courseCode}");

            courseTypesApiClient.Verify(x => x.Get<GetLearnerAgeResponse>(It.IsAny<GetLearnerAgeRequest>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task GetCourseTypeRulesAsync_WhenLearnerAgeRulesNotFound_ThrowsException(
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
            [Frozen] Mock<ICourseTypesApiClient> courseTypesApiClient,
            GetStandardsListItem learningResponse,
            string courseCode,
            CourseTypeRulesService service)
        {
            // Arrange
            learningResponse.ApprenticeshipType = "Apprenticeship";

            coursesApiClient
                .Setup(x => x.Get<GetStandardsListItem>(It.IsAny<GetStandardDetailsByIdRequest>()))
                .ReturnsAsync(learningResponse);

            courseTypesApiClient
                .Setup(x => x.Get<GetLearnerAgeResponse>(It.IsAny<GetLearnerAgeRequest>()))
                .ReturnsAsync((GetLearnerAgeResponse)null);

            // Act
            var act = () => service.GetCourseTypeRulesAsync(courseCode);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Learner age rules not found for apprenticeship type Apprenticeship");

            coursesApiClient.Verify(x => x.Get<GetStandardsListItem>(It.Is<GetStandardDetailsByIdRequest>(r => r.Id == courseCode)), Times.Once);
            courseTypesApiClient.Verify(x => x.Get<GetLearnerAgeResponse>(It.Is<GetLearnerAgeRequest>(r => r.GetUrl.Contains(learningResponse.ApprenticeshipType))), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task GetCourseTypeRulesAsync_WhenCoursesApiThrowsException_PropagatesException(
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
            [Frozen] Mock<ICourseTypesApiClient> courseTypesApiClient,
            string courseCode,
            CourseTypeRulesService service)
        {
            // Arrange
            var expectedException = new Exception("Courses API error");
            coursesApiClient
                .Setup(x => x.Get<GetStandardsListItem>(It.IsAny<GetStandardDetailsByIdRequest>()))
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
            GetStandardsListItem courseResponse,
            string courseCode,
            CourseTypeRulesService service)
        {
            // Arrange
            courseResponse.ApprenticeshipType = "Apprenticeship";
            var expectedException = new Exception("Course Types API error");

            coursesApiClient
                .Setup(x => x.Get<GetStandardsListItem>(It.IsAny<GetStandardDetailsByIdRequest>()))
                .ReturnsAsync(courseResponse);

            courseTypesApiClient
                .Setup(x => x.Get<GetLearnerAgeResponse>(It.IsAny<GetLearnerAgeRequest>()))
                .ThrowsAsync(expectedException);

            // Act
            var act = () => service.GetCourseTypeRulesAsync(courseCode);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .Where(e => e.Message == "Course Types API error");

            coursesApiClient.Verify(x => x.Get<GetStandardsListItem>(It.Is<GetStandardDetailsByIdRequest>(r => r.Id == courseCode)), Times.Once);
            courseTypesApiClient.Verify(x => x.Get<GetLearnerAgeResponse>(It.Is<GetLearnerAgeRequest>(r => r.GetUrl.Contains(courseResponse.ApprenticeshipType))), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task GetRplRulesAsync_WhenStandardAndRplRulesFound_ReturnsResult(
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
            [Frozen] Mock<ICourseTypesApiClient> courseTypesApiClient,
            GetStandardsListItem courseResponse,
            GetRecognitionOfPriorLearningResponse rplResponse,
            string courseCode,
            CourseTypeRulesService service)
        {
            // Arrange
            courseResponse.ApprenticeshipType = "Apprenticeship";

            coursesApiClient
                .Setup(x => x.Get<GetStandardsListItem>(It.IsAny<GetStandardDetailsByIdRequest>()))
                .ReturnsAsync(courseResponse);

            courseTypesApiClient
                .Setup(x => x.Get<GetRecognitionOfPriorLearningResponse>(It.IsAny<GetRecognitionOfPriorLearningRequest>()))
                .ReturnsAsync(rplResponse);

            // Act
            var result = await service.GetRplRulesAsync(courseCode);

            // Assert
            result.Should().NotBeNull();
            result.Course.Should().BeEquivalentTo(courseResponse, o => o.ExcludingMissingMembers().Excluding(c => c.LarsCode));
            result.Course.LarsCode.Should().Be(courseResponse.LarsCode.ToString());
            result.Course.LearningType.Should().Be(courseResponse.ApprenticeshipType);
            result.RplRules.Should().BeEquivalentTo(rplResponse);

            coursesApiClient.Verify(x => x.Get<GetStandardsListItem>(It.Is<GetStandardDetailsByIdRequest>(r => r.Id == courseCode)), Times.Once);
            courseTypesApiClient.Verify(x => x.Get<GetRecognitionOfPriorLearningResponse>(It.Is<GetRecognitionOfPriorLearningRequest>(r => r.GetUrl.Contains(courseResponse.ApprenticeshipType))), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task GetRplRulesAsync_WhenStandardNotFound_ThrowsException(
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
            [Frozen] Mock<ICourseTypesApiClient> courseTypesApiClient,
            string courseCode,
            CourseTypeRulesService service)
        {
            // Arrange
            coursesApiClient
                .Setup(x => x.Get<GetStandardsListItem>(It.IsAny<GetStandardDetailsByIdRequest>()))
                .ReturnsAsync((GetStandardsListItem)null);

            // Act
            var act = () => service.GetRplRulesAsync(courseCode);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage($"Standard not found for course ID {courseCode}");

            courseTypesApiClient.Verify(x => x.Get<GetRecognitionOfPriorLearningResponse>(It.IsAny<GetRecognitionOfPriorLearningRequest>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task GetRplRulesAsync_WhenRplRulesNotFound_ThrowsException(
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
            [Frozen] Mock<ICourseTypesApiClient> courseTypesApiClient,
            GetStandardsListItem courseResponse,
            string courseCode,
            CourseTypeRulesService service)
        {
            // Arrange
            courseResponse.ApprenticeshipType = "Apprenticeship";

            coursesApiClient
                .Setup(x => x.Get<GetStandardsListItem>(It.IsAny<GetStandardDetailsByIdRequest>()))
                .ReturnsAsync(courseResponse);

            courseTypesApiClient
                .Setup(x => x.Get<GetRecognitionOfPriorLearningResponse>(It.IsAny<GetRecognitionOfPriorLearningRequest>()))
                .ReturnsAsync((GetRecognitionOfPriorLearningResponse)null);

            // Act
            var act = () => service.GetRplRulesAsync(courseCode);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("RPL rules not found for apprenticeship type Apprenticeship");

            coursesApiClient.Verify(x => x.Get<GetStandardsListItem>(It.Is<GetStandardDetailsByIdRequest>(r => r.Id == courseCode)), Times.Once);
            courseTypesApiClient.Verify(x => x.Get<GetRecognitionOfPriorLearningResponse>(It.Is<GetRecognitionOfPriorLearningRequest>(r => r.GetUrl.Contains(courseResponse.ApprenticeshipType))), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task GetRplRulesAsync_WhenCoursesApiThrowsException_PropagatesException(
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
            [Frozen] Mock<ICourseTypesApiClient> courseTypesApiClient,
            string courseCode,
            CourseTypeRulesService service)
        {
            // Arrange
            var expectedException = new Exception("Courses API error");
            coursesApiClient
                .Setup(x => x.Get<GetStandardsListItem>(It.IsAny<GetStandardDetailsByIdRequest>()))
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
            GetStandardsListItem courseResponse,
            string courseCode,
            CourseTypeRulesService service)
        {
            // Arrange
            courseResponse.ApprenticeshipType = "Apprenticeship";
            var expectedException = new Exception("Course Types API error");

            coursesApiClient
                .Setup(x => x.Get<GetStandardsListItem>(It.IsAny<GetStandardDetailsByIdRequest>()))
                .ReturnsAsync(courseResponse);

            courseTypesApiClient
                .Setup(x => x.Get<GetRecognitionOfPriorLearningResponse>(It.IsAny<GetRecognitionOfPriorLearningRequest>()))
                .ThrowsAsync(expectedException);

            // Act
            var act = () => service.GetRplRulesAsync(courseCode);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .Where(e => e.Message == "Course Types API error");

            coursesApiClient.Verify(x => x.Get<GetStandardsListItem>(It.Is<GetStandardDetailsByIdRequest>(r => r.Id == courseCode)), Times.Once);
            courseTypesApiClient.Verify(x => x.Get<GetRecognitionOfPriorLearningResponse>(It.Is<GetRecognitionOfPriorLearningRequest>(r => r.GetUrl.Contains(courseResponse.ApprenticeshipType))), Times.Once);
        }
    }
}