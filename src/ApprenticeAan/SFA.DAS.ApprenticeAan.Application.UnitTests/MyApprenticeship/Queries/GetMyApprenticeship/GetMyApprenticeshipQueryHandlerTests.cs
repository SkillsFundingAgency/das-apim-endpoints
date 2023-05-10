using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Standards.Requests;
using SFA.DAS.ApprenticeAan.Application.MyApprenticeship.Queries.GetMyApprenticeship;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.MyApprenticeship.Queries.GetMyApprenticeship;

public class GetMyApprenticeshipQueryHandlerTests
{
    [Test]
    [MoqAutoData]
    public async Task Handle_ApprenticeWithMyApprenticeshipFoundWithStandardUidSet(
        [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apprenticeAccountsApiClientMock,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClientMock,
        GetMyApprenticeshipQueryHandler sut,
        GetMyApprenticeshipQuery request,
        MyApprenticeshipResponse response,
        CancellationToken cancellationToken)
    {
        const string StandardUid = "ST0418_1.0";
        const string TrainingCode = null!;
        const string StandardRoute = "route of standard";
        const int Duration = 36;
        const string Title = "Title";
        const int Level = 3;
        response.StandardUId = StandardUid;
        response.TrainingCode = TrainingCode;
  
        apprenticeAccountsApiClientMock.Setup(c => c.GetWithResponseCode<MyApprenticeshipResponse>(It.Is<GetMyApprenticeshipRequest>(r => r.Id == request.ApprenticeId)))!.ReturnsAsync(new ApiResponse<MyApprenticeshipResponse>(response, HttpStatusCode.OK, null));

        var standardResponse = new GetStandardResponse
        {
            Title = Title,
            Level = Level,
            Route = StandardRoute,
            VersionDetail = new StandardVersionDetail { ProposedTypicalDuration = Duration }
        };

        coursesApiClientMock.Setup(c => c.Get<GetStandardResponse>(It.IsAny<GetStandardQueryRequest>()))
            .ReturnsAsync(standardResponse);
            
        coursesApiClientMock.Setup(c => c.Get<GetFrameworkResponse>(It.IsAny<GetFrameworkQueryRequest>()))
            .ReturnsAsync(new GetFrameworkResponse());
        
        var actualResult = await sut.Handle(request, cancellationToken);
        
        apprenticeAccountsApiClientMock.Verify(c => c.GetWithResponseCode<MyApprenticeshipResponse>(It.Is<GetMyApprenticeshipRequest>(r => r.Id == request.ApprenticeId)));
        coursesApiClientMock.Verify(c=>c.Get<GetStandardResponse>(It.IsAny<GetStandardQueryRequest>()),Times.Once);
        coursesApiClientMock.Verify(c => c.Get<GetFrameworkResponse>(It.IsAny<GetFrameworkQueryRequest>()), Times.Never);
        actualResult?.MyApprenticeship.Should().NotBeNull();
        actualResult!.MyApprenticeship!.TrainingCourse.Should().BeEquivalentTo((TrainingCourse)standardResponse);
        actualResult.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [MoqAutoData]
    public async Task Handle_ApprenticeWithMyApprenticeshipFoundWithTrainingCodeSet(
        [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apprenticeAccountsApiClientMock,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClientMock,
        GetMyApprenticeshipQueryHandler sut,
        GetMyApprenticeshipQuery request,
        MyApprenticeshipResponse response,
        CancellationToken cancellationToken)
    {
        const string StandardUid = null!;
        const string TrainingCode = "403-2-1";
        const string FrameworkName = "name of framework";
        const int Duration = 36;
        const string Title = "Title";
        const int Level = 3;
        response.StandardUId = StandardUid;
        response.TrainingCode = TrainingCode;
        
        apprenticeAccountsApiClientMock.Setup(c => c.GetWithResponseCode<MyApprenticeshipResponse>(It.Is<GetMyApprenticeshipRequest>(r => r.Id == request.ApprenticeId)))!.ReturnsAsync(new ApiResponse<MyApprenticeshipResponse>(response, HttpStatusCode.OK, null));

        var frameworkResponse = new GetFrameworkResponse()
        {
            Title = Title,
            Level = Level,
            FrameworkName = FrameworkName,
            Duration = Duration
        };

        coursesApiClientMock.Setup(c => c.Get<GetStandardResponse>(It.IsAny<GetStandardQueryRequest>()))
            .ReturnsAsync(new GetStandardResponse());
        coursesApiClientMock.Setup(c => c.Get<GetFrameworkResponse>(It.IsAny<GetFrameworkQueryRequest>()))
            .ReturnsAsync(frameworkResponse);
        
        var actualResult = await sut.Handle(request, cancellationToken);
        
        apprenticeAccountsApiClientMock.Verify(c => c.GetWithResponseCode<MyApprenticeshipResponse>(It.Is<GetMyApprenticeshipRequest>(r => r.Id == request.ApprenticeId)));
        coursesApiClientMock.Verify(c => c.Get<GetStandardResponse>(It.IsAny<GetStandardQueryRequest>()), Times.Never);
        coursesApiClientMock.Verify(c => c.Get<GetFrameworkResponse>(It.IsAny<GetFrameworkQueryRequest>()), Times.Once);
        actualResult?.MyApprenticeship.Should().NotBeNull();
        actualResult!.MyApprenticeship!.TrainingCourse.Should().BeEquivalentTo((TrainingCourse)frameworkResponse);
        actualResult.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    [MoqAutoData]
    public async Task Handle_ApprenticeNotFound_ReturnsNotFound(
        [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apprenticeAccountsApiClientMock, 
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClientMock, 
        GetMyApprenticeshipQueryHandler sut, 
        GetMyApprenticeshipQuery request, 
        MyApprenticeshipResponse response,
        CancellationToken cancellationToken)
    {

        apprenticeAccountsApiClientMock.Setup(c => c.GetWithResponseCode<MyApprenticeshipResponse>(It.Is<GetMyApprenticeshipRequest>(r => r.Id == request.ApprenticeId)))!.ReturnsAsync(new ApiResponse<MyApprenticeshipResponse>(response, HttpStatusCode.NotFound, null));
        var actualResult = await sut.Handle(request, cancellationToken);

        apprenticeAccountsApiClientMock.Verify(c => c.GetWithResponseCode<MyApprenticeshipResponse>(It.Is<GetMyApprenticeshipRequest>(r => r.Id == request.ApprenticeId)));
        coursesApiClientMock.Verify(c => c.Get<GetStandardResponse>(It.IsAny<GetStandardQueryRequest>()), Times.Never);
        coursesApiClientMock.Verify(c => c.Get<GetFrameworkResponse>(It.IsAny<GetFrameworkQueryRequest>()), Times.Never);
        actualResult.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    [MoqAutoData]
    public async Task Handle_ApprenticeBadRequest_ReturnsBadRequest(
        [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apprenticeAccountsApiClientMock,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClientMock,
        GetMyApprenticeshipQueryHandler sut,
        GetMyApprenticeshipQuery request,
        MyApprenticeshipResponse response,
        CancellationToken cancellationToken)
    {

        apprenticeAccountsApiClientMock.Setup(c => c.GetWithResponseCode<MyApprenticeshipResponse>(It.Is<GetMyApprenticeshipRequest>(r => r.Id == request.ApprenticeId)))!.ReturnsAsync(new ApiResponse<MyApprenticeshipResponse>(response, HttpStatusCode.BadRequest, null));
        var actualResult = await sut.Handle(request, cancellationToken);

        apprenticeAccountsApiClientMock.Verify(c => c.GetWithResponseCode<MyApprenticeshipResponse>(It.Is<GetMyApprenticeshipRequest>(r => r.Id == request.ApprenticeId)));
        coursesApiClientMock.Verify(c => c.Get<GetStandardResponse>(It.IsAny<GetStandardQueryRequest>()), Times.Never);
        coursesApiClientMock.Verify(c => c.Get<GetFrameworkResponse>(It.IsAny<GetFrameworkQueryRequest>()), Times.Never);
        actualResult.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}