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

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.MyApprenticeship.Queries.GetMyApprenticeship
{
    public class GetMyApprenticeshipQueryHandlerTests
    {
        [Test]
        [MoqAutoData]
        public async Task Handle_ApprenticeWithoutMyApprenticeshipsFound(
            [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apprenticeAccountsApiClientMock,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClientMock,
            GetMyApprenticeshipQueryHandler sut,
            GetMyApprenticeshipQuery request,
            GetMyApprenticeshipsQueryResponse expectedResult,
            CancellationToken cancellationToken)
        {
            expectedResult.MyApprenticeships = null;
            apprenticeAccountsApiClientMock.Setup(c => c.GetWithResponseCode<GetMyApprenticeshipsQueryResponse>(It.Is<GetMyApprenticeshipRequest>(r => r.Id == request.ApprenticeId)))!.ReturnsAsync(new ApiResponse<GetMyApprenticeshipsQueryResponse>(expectedResult, HttpStatusCode.OK, null));
          
            coursesApiClientMock.Setup(c => c.Get<GetStandardResponse>(It.IsAny<GetStandardQueryRequest>()))
                .ReturnsAsync(new GetStandardResponse());
            coursesApiClientMock.Setup(c => c.Get<GetFrameworkResponse>(It.IsAny<GetFrameworkQueryRequest>()))
                .ReturnsAsync(new GetFrameworkResponse());

            var actualResult = await sut.Handle(request, cancellationToken);

            apprenticeAccountsApiClientMock.Verify(c => c.GetWithResponseCode<GetMyApprenticeshipsQueryResponse>(It.Is<GetMyApprenticeshipRequest>(r => r.Id == request.ApprenticeId)));
            coursesApiClientMock.Verify(c=>c.Get<GetStandardResponse>(It.IsAny<GetStandardQueryRequest>()),Times.Never);
            coursesApiClientMock.Verify(c => c.Get<GetFrameworkResponse>(It.IsAny<GetFrameworkQueryRequest>()), Times.Never);
            actualResult.Should().BeEquivalentTo((GetMyApprenticeshipQueryResult)expectedResult);
        }

        [Test]
        [MoqAutoData]
        public async Task Handle_ApprenticeNotFound_ReturnsNull(
            [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apiClientMock,
            GetMyApprenticeshipQueryHandler sut,
            GetMyApprenticeshipQuery request,
            CancellationToken cancellationToken)
        {
            apiClientMock.Setup(c => c.GetWithResponseCode<GetMyApprenticeshipsQueryResponse?>(It.Is<GetMyApprenticeshipRequest>(r => r.Id == request.ApprenticeId))).ReturnsAsync(new ApiResponse<GetMyApprenticeshipsQueryResponse?>(null, HttpStatusCode.NotFound, null));
            var actualResult = await sut.Handle(request, cancellationToken);
            Assert.That(actualResult, Is.Null);
        }

        [Test]
        [MoqAutoData]
        public async Task Handle_ApiInvalidResponse_ThrowsInvalidOperationException(
            [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apiClientMock,
            GetMyApprenticeshipQueryHandler sut,
            GetMyApprenticeshipQuery request,
            CancellationToken cancellationToken)
        {
            apiClientMock.Setup(c => c.GetWithResponseCode<GetMyApprenticeshipsQueryResponse?>(It.Is<GetMyApprenticeshipRequest>(r => r.Id == request.ApprenticeId))).ReturnsAsync(new ApiResponse<GetMyApprenticeshipsQueryResponse?>(null, HttpStatusCode.InternalServerError, null));
            Func<Task> act = () => sut.Handle(request, cancellationToken);
            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Test]
        [MoqAutoData]
        public async Task Handle_ApprenticeWithMyApprenticeshipsAndStandardFound(
            [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apprenticeAccountsApiClientMock,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClientMock,
            GetMyApprenticeshipQueryHandler sut,
            GetMyApprenticeshipQuery request,
            GetMyApprenticeshipsQueryResponse expectedResult,
            GetStandardResponse standardResponse,
            GetFrameworkResponse frameworkResponse,
            List<MyApprenticeshipResponse> myApprenticeships,
            CancellationToken cancellationToken)
        {
            expectedResult.MyApprenticeships = myApprenticeships;
            var standardUid = myApprenticeships.First().StandardUId;
            var trainingCode = myApprenticeships.First().TrainingCode;

            apprenticeAccountsApiClientMock.Setup(c => c.GetWithResponseCode<GetMyApprenticeshipsQueryResponse>(It.Is<GetMyApprenticeshipRequest>(r => r.Id == request.ApprenticeId)))!.ReturnsAsync(new ApiResponse<GetMyApprenticeshipsQueryResponse>(expectedResult, HttpStatusCode.OK, null));
        
            coursesApiClientMock.Setup(c => c.Get<GetStandardResponse>(It.Is<GetStandardQueryRequest>(r=>r.StandardUid==standardUid)))
                .ReturnsAsync(standardResponse);
            coursesApiClientMock.Setup(c => c.Get<GetFrameworkResponse>(It.Is<GetFrameworkQueryRequest>(r => r.TrainingCode == trainingCode)))
                .ReturnsAsync(frameworkResponse);

            var actualResult = await sut.Handle(request, cancellationToken);
        
            apprenticeAccountsApiClientMock.Verify(c => c.GetWithResponseCode<GetMyApprenticeshipsQueryResponse>(It.Is<GetMyApprenticeshipRequest>(r => r.Id == request.ApprenticeId)));
            coursesApiClientMock.Verify(c => c.Get<GetStandardResponse>(It.IsAny<GetStandardQueryRequest>()), Times.Once);
            coursesApiClientMock.Verify(c => c.Get<GetFrameworkResponse>(It.IsAny<GetFrameworkQueryRequest>()), Times.Never);
            actualResult.Should().BeEquivalentTo((GetMyApprenticeshipQueryResult)expectedResult, l=>l.Excluding(e=>e.MyApprenticeship));

            actualResult!.MyApprenticeship!.TrainingCourse.Should().BeEquivalentTo((TrainingCourse)standardResponse);
        }


        [Test]
        [MoqAutoData]
        public async Task Handle_ApprenticeWithMyApprenticeshipsAndFrameworkFound(
            [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apprenticeAccountsApiClientMock,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClientMock,
            GetMyApprenticeshipQueryHandler sut,
            GetMyApprenticeshipQuery request,
            GetMyApprenticeshipsQueryResponse expectedResult,
            GetFrameworkResponse frameworkResponse,
            List<MyApprenticeshipResponse> myApprenticeships,
            CancellationToken cancellationToken)
        {
            expectedResult.MyApprenticeships = myApprenticeships;
            myApprenticeships.First().StandardUId = null;
            var trainingCode = myApprenticeships.First().TrainingCode;

            apprenticeAccountsApiClientMock.Setup(c => c.GetWithResponseCode<GetMyApprenticeshipsQueryResponse>(It.Is<GetMyApprenticeshipRequest>(r => r.Id == request.ApprenticeId)))!.ReturnsAsync(new ApiResponse<GetMyApprenticeshipsQueryResponse>(expectedResult, HttpStatusCode.OK, null));

            coursesApiClientMock.Setup(c => c.Get<GetStandardResponse>(It.IsAny<GetStandardQueryRequest>()))
                .ReturnsAsync(new GetStandardResponse());
            coursesApiClientMock.Setup(c => c.Get<GetFrameworkResponse>(It.Is<GetFrameworkQueryRequest>(r => r.TrainingCode == trainingCode)))
                .ReturnsAsync(frameworkResponse);

            var actualResult = await sut.Handle(request, cancellationToken);

            apprenticeAccountsApiClientMock.Verify(c => c.GetWithResponseCode<GetMyApprenticeshipsQueryResponse>(It.Is<GetMyApprenticeshipRequest>(r => r.Id == request.ApprenticeId)));
            coursesApiClientMock.Verify(c => c.Get<GetStandardResponse>(It.IsAny<GetStandardQueryRequest>()), Times.Never);
            coursesApiClientMock.Verify(c => c.Get<GetFrameworkResponse>(It.IsAny<GetFrameworkQueryRequest>()), Times.Once);
            actualResult.Should().BeEquivalentTo((GetMyApprenticeshipQueryResult)expectedResult, l => l.Excluding(e => e.MyApprenticeship));
            actualResult!.MyApprenticeship!.TrainingCourse.Should().BeEquivalentTo((TrainingCourse)frameworkResponse);
        }
    }
}
