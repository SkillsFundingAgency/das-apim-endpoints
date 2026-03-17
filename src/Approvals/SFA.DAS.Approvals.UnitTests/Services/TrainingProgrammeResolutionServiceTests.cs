using System;
using System.Threading.Tasks;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Courses;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Services;

[TestFixture]
public class TrainingProgrammeResolutionServiceTests
{
    [Test, MoqAutoData]
    public async Task GetTrainingProgrammeAsync_WhenCourseCodeIsNull_ReturnsNull(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        TrainingProgrammeResolutionService service)
    {
        // Act
        var result = await service.GetTrainingProgrammeAsync(null, DateTime.UtcNow);

        // Assert
        result.Should().BeNull();
        commitmentsApiClient.VerifyNoOtherCalls();
    }

    [Test, MoqAutoData]
    public async Task GetTrainingProgrammeAsync_WhenCourseCodeIsEmpty_ReturnsNull(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        TrainingProgrammeResolutionService service,
        DateTime? startDate)
    {
        var result = await service.GetTrainingProgrammeAsync(string.Empty, startDate);

        result.Should().BeNull();
        commitmentsApiClient.VerifyNoOtherCalls();
    }

    [Test, MoqAutoData]
    public async Task GetTrainingProgrammeAsync_WhenCourseCodeIsWhiteSpace_ReturnsNull(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        TrainingProgrammeResolutionService service)
    {
        var result = await service.GetTrainingProgrammeAsync("   ", new DateTime(2024, 9, 1));

        result.Should().BeNull();
        commitmentsApiClient.VerifyNoOtherCalls();
    }

    [Test, MoqAutoData]
    public async Task GetTrainingProgrammeAsync_WhenNumericCourseCodeAndStartDate_CallsCalculateVersionAndReturnsResponse(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        TrainingProgrammeResolutionService service,
        GetTrainingProgrammeResponse apiResponse,
        DateTime startDate)
    {
        const string courseCode = "123";
        commitmentsApiClient
            .Setup(x => x.Get<GetTrainingProgrammeResponse>(
                It.Is<GetCalculatedTrainingProgrammeVersionRequest>(r => r.GetUrl.Contains("123")
                                                                         && r.GetUrl.Contains(startDate.ToString("O", System.Globalization.CultureInfo.InvariantCulture)))))
            .ReturnsAsync(apiResponse)
            .Verifiable();

        var result = await service.GetTrainingProgrammeAsync(courseCode, startDate);

        result.Should().Be(apiResponse);
        commitmentsApiClient.Verify();
        commitmentsApiClient.VerifyNoOtherCalls();
    }

    [Test, MoqAutoData]
    public async Task GetTrainingProgrammeAsync_WhenNumericCourseCodeButNoStartDate_CallsGetTrainingProgrammeByCode(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        TrainingProgrammeResolutionService service,
        GetTrainingProgrammeResponse apiResponse)
    {
        const string courseCode = "456";
        commitmentsApiClient
            .Setup(x => x.Get<GetTrainingProgrammeResponse>(
                It.Is<GetTrainingProgrammeRequest>(r => r.CourseCode == courseCode)))
            .ReturnsAsync(apiResponse)
            .Verifiable();

        var result = await service.GetTrainingProgrammeAsync(courseCode, null);

        result.Should().Be(apiResponse);
        commitmentsApiClient.Verify();
        commitmentsApiClient.VerifyNoOtherCalls();
    }

    [Test, MoqAutoData]
    public async Task GetTrainingProgrammeAsync_WhenNonNumericCourseCode_CallsGetTrainingProgrammeByCode(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        TrainingProgrammeResolutionService service,
        GetTrainingProgrammeResponse apiResponse,
        DateTime startDate)
    {
        const string courseCode = "550-2-1";
        commitmentsApiClient
            .Setup(x => x.Get<GetTrainingProgrammeResponse>(
                It.Is<GetTrainingProgrammeRequest>(r => r.CourseCode == courseCode)))
            .ReturnsAsync(apiResponse)
            .Verifiable();

        var result = await service.GetTrainingProgrammeAsync(courseCode, startDate);

        result.Should().Be(apiResponse);
        commitmentsApiClient.Verify();
        commitmentsApiClient.VerifyNoOtherCalls();
    }

    [Test, MoqAutoData]
    public async Task GetTrainingProgrammeAsync_WhenCourseCodeIsZeroAndStartDateProvided_CallsGetTrainingProgrammeByCode(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        TrainingProgrammeResolutionService service,
        GetTrainingProgrammeResponse apiResponse,
        DateTime startDate)
    {
        const string courseCode = "0";
        commitmentsApiClient
            .Setup(x => x.Get<GetTrainingProgrammeResponse>(
                It.Is<GetTrainingProgrammeRequest>(r => r.CourseCode == courseCode)))
            .ReturnsAsync(apiResponse)
            .Verifiable();

        var result = await service.GetTrainingProgrammeAsync(courseCode, startDate);

        result.Should().Be(apiResponse);
        commitmentsApiClient.Verify();
        commitmentsApiClient.VerifyNoOtherCalls();
    }

    [Test, MoqAutoData]
    public async Task GetTrainingProgrammeAsync_WhenApiThrows_PropagatesException(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        TrainingProgrammeResolutionService service)
    {
        const string courseCode = "123";
        var startDate = new DateTime(2024, 9, 1);
        var expectedException = new InvalidOperationException("Commitments API error");
        commitmentsApiClient
            .Setup(x => x.Get<GetTrainingProgrammeResponse>(
                It.Is<GetCalculatedTrainingProgrammeVersionRequest>(r => r.GetUrl.Contains(courseCode)
                                                                         && r.GetUrl.Contains(startDate.ToString("O", System.Globalization.CultureInfo.InvariantCulture)))))
            .ThrowsAsync(expectedException)
            .Verifiable();

        var act = () => service.GetTrainingProgrammeAsync(courseCode, startDate);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Commitments API error");
        commitmentsApiClient.Verify();
        commitmentsApiClient.VerifyNoOtherCalls();
    }
}
