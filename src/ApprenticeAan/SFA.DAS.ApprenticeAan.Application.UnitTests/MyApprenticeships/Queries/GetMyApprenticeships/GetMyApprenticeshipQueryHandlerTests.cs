using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.InnerApi.MyApprenticeships;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Standards.Responses;
using SFA.DAS.ApprenticeAan.Application.MyApprenticeships.Queries.GetMyApprenticeship;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.MyApprenticeships.Queries.GetMyApprenticeships;

public class GetMyApprenticeshipQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_MyApprenticeshipFoundWithStandardUidSet(
        [Frozen] Mock<IApprenticeAccountsApiClient> apprenticeAccountsApiClientMock,
        [Frozen] Mock<ICoursesApiClient> coursesApiClientMock,
        GetMyApprenticeshipQueryHandler sut,
        GetMyApprenticeshipQuery request,
        GetMyApprenticeshipResponse response,
        CancellationToken cancellationToken)
    {
        const string standardUid = "ST0418_1.0";
        const string trainingCode = null!;
        const string standardRoute = "route of standard";
        const int duration = 36;
        const string title = "Title";
        const int Level = 3;
        response.StandardUId = standardUid;
        response.TrainingCode = trainingCode;

        var status = System.Net.HttpStatusCode.OK;

        var restApprenticeshipResponse = new RestEase.Response<GetMyApprenticeshipResponse>(
            "not used",
            new HttpResponseMessage(status),
            () => response);

        apprenticeAccountsApiClientMock.Setup(c => c.GetMyApprenticeship(request.ApprenticeId, It.IsAny<CancellationToken>())).ReturnsAsync(restApprenticeshipResponse);

        var standardResponse = new GetStandardResponse
        {
            Title = title,
            Level = Level,
            Route = standardRoute,
            VersionDetail = new StandardVersionDetail { ProposedTypicalDuration = duration }
        };

        var restStandardResponse = new RestEase.Response<GetStandardResponse>(
            "not used",
            new HttpResponseMessage(status),
            () => standardResponse);

        coursesApiClientMock.Setup(c => c.GetStandard(standardUid, It.IsAny<CancellationToken>())).ReturnsAsync(restStandardResponse);

        var restFrameworkResponse = new RestEase.Response<GetFrameworkResponse>(
            "not used",
            new HttpResponseMessage(status),
            () => new GetFrameworkResponse());

        coursesApiClientMock.Setup(c => c.GetFramework(trainingCode, It.IsAny<CancellationToken>())).ReturnsAsync(restFrameworkResponse);

        var actualResult = await sut.Handle(request, cancellationToken);

        apprenticeAccountsApiClientMock.Verify(c => c.GetMyApprenticeship(request.ApprenticeId, It.IsAny<CancellationToken>()));
        coursesApiClientMock.Verify(c => c.GetStandard(standardUid, It.IsAny<CancellationToken>()), Times.Once);
        coursesApiClientMock.Verify(c => c.GetFramework(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        actualResult?.Should().NotBeNull();
        actualResult!.TrainingCourse.Should().BeEquivalentTo((TrainingCourse)standardResponse);
    }

    [Test, MoqAutoData]
    public async Task Handle_MyApprenticeshipFoundWithTrainingCodeSet(
      [Frozen] Mock<IApprenticeAccountsApiClient> apprenticeAccountsApiClientMock,
      [Frozen] Mock<ICoursesApiClient> coursesApiClientMock,
      GetMyApprenticeshipQueryHandler sut,
      GetMyApprenticeshipQuery request,
      GetMyApprenticeshipResponse response,
      CancellationToken cancellationToken)
    {
        const string standardUid = null!;
        const string trainingCode = "403-2-1";
        const string frameworkName = "name of framework";

        const int duration = 36;
        const string title = "Title";
        const int Level = 3;
        response.StandardUId = standardUid;
        response.TrainingCode = trainingCode;

        var status = HttpStatusCode.OK;

        var restApprenticeshipResponse = new RestEase.Response<GetMyApprenticeshipResponse>(
            "not used",
            new HttpResponseMessage(status),
            () => response);

        apprenticeAccountsApiClientMock.Setup(c => c.GetMyApprenticeship(request.ApprenticeId, It.IsAny<CancellationToken>())).ReturnsAsync(restApprenticeshipResponse);

        var frameworkResponse = new GetFrameworkResponse()
        {
            Title = title,
            Level = Level,
            FrameworkName = frameworkName,
            Duration = duration
        };

        var restStandardResponse = new RestEase.Response<GetStandardResponse>(
            "not used",
            new HttpResponseMessage(status),
            () => new GetStandardResponse());

        coursesApiClientMock.Setup(c => c.GetStandard(standardUid, It.IsAny<CancellationToken>())).ReturnsAsync(restStandardResponse);

        var restFrameworkResponse = new RestEase.Response<GetFrameworkResponse>(
            "not used",
            new HttpResponseMessage(status),
            () => frameworkResponse);

        coursesApiClientMock.Setup(c => c.GetFramework(trainingCode, It.IsAny<CancellationToken>())).ReturnsAsync(restFrameworkResponse);

        var actualResult = await sut.Handle(request, cancellationToken);

        apprenticeAccountsApiClientMock.Verify(c => c.GetMyApprenticeship(request.ApprenticeId, It.IsAny<CancellationToken>()));
        coursesApiClientMock.Verify(c => c.GetStandard(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        coursesApiClientMock.Verify(c => c.GetFramework(trainingCode, It.IsAny<CancellationToken>()), Times.Once);
        actualResult?.Should().NotBeNull();
        actualResult!.TrainingCourse.Should().BeEquivalentTo((TrainingCourse)frameworkResponse);
    }

    // [Test, MoqAutoData]
    // public async Task Handle_ApprenticeNotFound_ReturnsNull(
    //     [Frozen] Mock<IApprenticeAccountsApiClient> apprenticeAccountsApiClientMock,
    //     [Frozen] Mock<ICoursesApiClient> coursesApiClientMock,
    //     GetMyApprenticeshipQueryHandler sut,
    //     GetMyApprenticeshipQuery request,
    //     GetMyApprenticeshipResponse response,
    //     CancellationToken cancellationToken)
    // {
    //     var status = HttpStatusCode.NotFound;
    //
    //     var restApprenticeshipResponse = new RestEase.Response<GetMyApprenticeshipResponse>(
    //         "not used",
    //         new HttpResponseMessage(status),
    //         () => response);
    //
    //     apprenticeAccountsApiClientMock.Setup(c => c.GetMyApprenticeship(request.ApprenticeId, It.IsAny<CancellationToken>())).ReturnsAsync(restApprenticeshipResponse);
    //     var actualResult = await sut.Handle(request, cancellationToken);
    //
    //     apprenticeAccountsApiClientMock.Verify(c => c.GetMyApprenticeship(request.ApprenticeId, It.IsAny<CancellationToken>()));
    //     coursesApiClientMock.Verify(c => c.GetStandard(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    //     coursesApiClientMock.Verify(c => c.GetFramework(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    //     actualResult.Should().BeNull();
    // }
    //
    // [Test, MoqAutoData]
    // public async Task Handle_ApiBadRequest_ThrowsInvalidOperationException(
    //     [Frozen] Mock<IApprenticeAccountsApiClient> apprenticeAccountsApiClientMock,
    //     [Frozen] Mock<ICoursesApiClient> coursesApiClientMock,
    //     GetMyApprenticeshipQueryHandler sut,
    //     GetMyApprenticeshipQuery request,
    //     CancellationToken cancellationToken)
    // {
    //     var status = HttpStatusCode.BadRequest;
    //
    //     var restApprenticeshipResponse = new RestEase.Response<GetMyApprenticeshipResponse>(
    //         "not used",
    //         new HttpResponseMessage(status),
    //         () => null!);
    //
    //     apprenticeAccountsApiClientMock.Setup(c => c.GetMyApprenticeship(request.ApprenticeId, It.IsAny<CancellationToken>())).ReturnsAsync(restApprenticeshipResponse);
    //
    //     Func<Task> act = () => sut.Handle(request, cancellationToken);
    //
    //     await act.Should().ThrowAsync<InvalidOperationException>();
    //     apprenticeAccountsApiClientMock.Verify(c => c.GetMyApprenticeship(request.ApprenticeId, It.IsAny<CancellationToken>()));
    //     coursesApiClientMock.Verify(c => c.GetStandard(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    //     coursesApiClientMock.Verify(c => c.GetFramework(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    // }
}