//MFCMFC
// using System.Net;
// using AutoFixture.NUnit3;
// using FluentAssertions;
// using Moq;
// using SFA.DAS.EmployerAan.Application.InnerApi.MyApprenticeships;
// using SFA.DAS.EmployerAan.Application.InnerApi.Standards.Requests;
// using SFA.DAS.EmployerAan.Application.MyApprenticeships.Queries.GetMyApprenticeship;
// using SFA.DAS.EmployerAan.InnerApi.MyApprenticeships;
// using SFA.DAS.SharedOuterApi.Configuration;
// using SFA.DAS.SharedOuterApi.Interfaces;
// using SFA.DAS.SharedOuterApi.Models;
// using SFA.DAS.Testing.AutoFixture;
//
// namespace SFA.DAS.EmployerAan.Application.UnitTests.MyApprenticeships.Queries.GetMyApprenticeships;
//
// public class GetMyApprenticeshipQueryHandlerTests
// {
//     [Test]
//     [MoqAutoData]
//     public async Task Handle_MyApprenticeshipFoundWithStandardUidSet(
//         [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apprenticeAccountsApiClientMock,
//         [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClientMock,
//         GetMyApprenticeshipQueryHandler sut,
//         GetMyApprenticeshipQuery request,
//         GetMyApprenticeshipResponse response,
//         CancellationToken cancellationToken)
//     {
//         const string StandardUid = "ST0418_1.0";
//         const string TrainingCode = null!;
//         const string StandardRoute = "route of standard";
//         const int Duration = 36;
//         const string Title = "Title";
//         const int Level = 3;
//         response.StandardUId = StandardUid;
//         response.TrainingCode = TrainingCode;
//
//         apprenticeAccountsApiClientMock.Setup(c => c.GetWithResponseCode<GetMyApprenticeshipResponse>(It.Is<GetMyApprenticeshipRequest>(r => r.Id == request.ApprenticeId)))!.ReturnsAsync(new ApiResponse<GetMyApprenticeshipResponse>(response, HttpStatusCode.OK, null));
//
//         var standardResponse = new GetStandardResponse
//         {
//             Title = Title,
//             Level = Level,
//             Route = StandardRoute,
//             VersionDetail = new StandardVersionDetail { ProposedTypicalDuration = Duration }
//         };
//
//         coursesApiClientMock.Setup(c => c.Get<GetStandardResponse>(It.IsAny<GetStandardQueryRequest>()))
//             .ReturnsAsync(standardResponse);
//
//         coursesApiClientMock.Setup(c => c.Get<GetFrameworkResponse>(It.IsAny<GetFrameworkQueryRequest>()))
//             .ReturnsAsync(new GetFrameworkResponse());
//
//         var actualResult = await sut.Handle(request, cancellationToken);
//
//         apprenticeAccountsApiClientMock.Verify(c => c.GetWithResponseCode<GetMyApprenticeshipResponse>(It.Is<GetMyApprenticeshipRequest>(r => r.Id == request.ApprenticeId)));
//         coursesApiClientMock.Verify(c => c.Get<GetStandardResponse>(It.IsAny<GetStandardQueryRequest>()), Times.Once);
//         coursesApiClientMock.Verify(c => c.Get<GetFrameworkResponse>(It.IsAny<GetFrameworkQueryRequest>()), Times.Never);
//         actualResult?.Should().NotBeNull();
//         actualResult!.TrainingCourse.Should().BeEquivalentTo((TrainingCourse)standardResponse);
//     }
//
//     [Test]
//     [MoqAutoData]
//     public async Task Handle_MyApprenticeshipFoundWithTrainingCodeSet(
//         [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apprenticeAccountsApiClientMock,
//         [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClientMock,
//         GetMyApprenticeshipQueryHandler sut,
//         GetMyApprenticeshipQuery request,
//         GetMyApprenticeshipResponse response,
//         CancellationToken cancellationToken)
//     {
//         const string StandardUid = null!;
//         const string TrainingCode = "403-2-1";
//         const string FrameworkName = "name of framework";
//         const int Duration = 36;
//         const string Title = "Title";
//         const int Level = 3;
//         response.StandardUId = StandardUid;
//         response.TrainingCode = TrainingCode;
//
//         apprenticeAccountsApiClientMock.Setup(c => c.GetWithResponseCode<GetMyApprenticeshipResponse>(It.Is<GetMyApprenticeshipRequest>(r => r.Id == request.ApprenticeId)))!.ReturnsAsync(new ApiResponse<GetMyApprenticeshipResponse>(response, HttpStatusCode.OK, null));
//
//         var frameworkResponse = new GetFrameworkResponse()
//         {
//             Title = Title,
//             Level = Level,
//             FrameworkName = FrameworkName,
//             Duration = Duration
//         };
//
//         coursesApiClientMock.Setup(c => c.Get<GetStandardResponse>(It.IsAny<GetStandardQueryRequest>()))
//             .ReturnsAsync(new GetStandardResponse());
//         coursesApiClientMock.Setup(c => c.Get<GetFrameworkResponse>(It.IsAny<GetFrameworkQueryRequest>()))
//             .ReturnsAsync(frameworkResponse);
//
//         var actualResult = await sut.Handle(request, cancellationToken);
//
//         apprenticeAccountsApiClientMock.Verify(c => c.GetWithResponseCode<GetMyApprenticeshipResponse>(It.Is<GetMyApprenticeshipRequest>(r => r.Id == request.ApprenticeId)));
//         coursesApiClientMock.Verify(c => c.Get<GetStandardResponse>(It.IsAny<GetStandardQueryRequest>()), Times.Never);
//         coursesApiClientMock.Verify(c => c.Get<GetFrameworkResponse>(It.IsAny<GetFrameworkQueryRequest>()), Times.Once);
//         actualResult?.Should().NotBeNull();
//         actualResult!.TrainingCourse.Should().BeEquivalentTo((TrainingCourse)frameworkResponse);
//     }
//
//     [Test]
//     [MoqAutoData]
//     public async Task Handle_ApprenticeNotFound_ReturnsNull(
//         [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apprenticeAccountsApiClientMock,
//         [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClientMock,
//         GetMyApprenticeshipQueryHandler sut,
//         GetMyApprenticeshipQuery request,
//         GetMyApprenticeshipResponse response,
//         CancellationToken cancellationToken)
//     {
//
//         apprenticeAccountsApiClientMock.Setup(c => c.GetWithResponseCode<GetMyApprenticeshipResponse>(It.Is<GetMyApprenticeshipRequest>(r => r.Id == request.ApprenticeId)))!.ReturnsAsync(new ApiResponse<GetMyApprenticeshipResponse>(response, HttpStatusCode.NotFound, null));
//         var actualResult = await sut.Handle(request, cancellationToken);
//
//         apprenticeAccountsApiClientMock.Verify(c => c.GetWithResponseCode<GetMyApprenticeshipResponse>(It.Is<GetMyApprenticeshipRequest>(r => r.Id == request.ApprenticeId)));
//         coursesApiClientMock.Verify(c => c.Get<GetStandardResponse>(It.IsAny<GetStandardQueryRequest>()), Times.Never);
//         coursesApiClientMock.Verify(c => c.Get<GetFrameworkResponse>(It.IsAny<GetFrameworkQueryRequest>()), Times.Never);
//         actualResult.Should().BeNull();
//     }
//
//     [Test]
//     [MoqAutoData]
//     public async Task Handle_ApiBadRequest_ThrowsInvalidOperationException(
//         [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apprenticeAccountsApiClientMock,
//         [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClientMock,
//         GetMyApprenticeshipQueryHandler sut,
//         GetMyApprenticeshipQuery request,
//         CancellationToken cancellationToken)
//     {
//         apprenticeAccountsApiClientMock.Setup(c => c.GetWithResponseCode<GetMyApprenticeshipResponse>(It.Is<GetMyApprenticeshipRequest>(r => r.Id == request.ApprenticeId)))!.ReturnsAsync(new ApiResponse<GetMyApprenticeshipResponse>(null!, HttpStatusCode.BadRequest, null));
//
//         Func<Task> act = () => sut.Handle(request, cancellationToken);
//
//         await act.Should().ThrowAsync<InvalidOperationException>();
//         apprenticeAccountsApiClientMock.Verify(c => c.GetWithResponseCode<GetMyApprenticeshipResponse>(It.Is<GetMyApprenticeshipRequest>(r => r.Id == request.ApprenticeId)));
//         coursesApiClientMock.Verify(c => c.Get<GetStandardResponse>(It.IsAny<GetStandardQueryRequest>()), Times.Never);
//         coursesApiClientMock.Verify(c => c.Get<GetFrameworkResponse>(It.IsAny<GetFrameworkQueryRequest>()), Times.Never);
//
//     }
// }