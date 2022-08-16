using AutoFixture;
using AutoFixture.NUnit3;
using Moq;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrackProgress.Apis.CommitmentsV2InnerApi;
using static SFA.DAS.TrackProgress.Apis.CommitmentsV2InnerApi.GetApprenticeshipsResponse;
using SFA.DAS.TrackProgress.Application.Commands;
using SFA.DAS.TrackProgress.Application.Services;
using System.Net;

namespace SFA.DAS.TrackProgress.OuterApi.Tests.HandlerTests
{
    public class TrackProgressCommandHandlerTests
    {
        private readonly Fixture _fixture = new Fixture();
        private TrackProgressCommandHandler? _sut = null;
        private CommitmentsV2Service? _service = null;

        [Test, MoqAutoData]
        public async Task TestFor201Result(
            [Frozen] Mock<IInternalApiClient<CommitmentsV2ApiConfiguration>> commitmentsV2Api)
        {
            //Arrange
            AddApprenticeshipResponse(ref commitmentsV2Api, HttpStatusCode.OK);

            _service = new CommitmentsV2Service(commitmentsV2Api.Object);
            _sut = new TrackProgressCommandHandler(_service);

            // Act
            var request = _fixture.Create<TrackProgressCommand>();
            var result = await _sut.Handle(request, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.InstanceOf(typeof(TrackProgressResponse)));
                Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            });   
        }

        [Test, MoqAutoData]
        public async Task TestFor404ResultWhenApprenticeshipNotFound(
            [Frozen] Mock<IInternalApiClient<CommitmentsV2ApiConfiguration>> commitmentsV2Api)
        {
            // Arrange            
            AddApprenticeshipResponse(ref commitmentsV2Api, HttpStatusCode.NotFound);

            _service = new CommitmentsV2Service(commitmentsV2Api.Object);
            _sut = new TrackProgressCommandHandler(_service);

            // Act
            var request = _fixture.Create<TrackProgressCommand>();
            var result = await _sut.Handle(request, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.InstanceOf(typeof(TrackProgressResponse)));
                Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test, MoqAutoData]
        public async Task TestFor404ResultWhenProviderNotFound(
            [Frozen] Mock<IInternalApiClient<CommitmentsV2ApiConfiguration>> commitmentsV2Api)
        {
            // Arrange
            AddProviderResponse(ref commitmentsV2Api, HttpStatusCode.NotFound);

            var apprenticeshipsResponse = _fixture.Build<GetApprenticeshipsResponse>()
                .With(x => x.StatusCode, HttpStatusCode.OK)
                .With(x => x.TotalApprenticeshipsFound, 0).Create();
            var apprenticeshipsApiResponse = new ApiResponse<GetApprenticeshipsResponse>(apprenticeshipsResponse, HttpStatusCode.OK, string.Empty);
            commitmentsV2Api.Setup(x => x.GetWithResponseCode<GetApprenticeshipsResponse>(It.IsAny<GetApprenticeshipsRequest>())).ReturnsAsync(apprenticeshipsApiResponse);

            _service = new CommitmentsV2Service(commitmentsV2Api.Object);
            _sut = new TrackProgressCommandHandler(_service);

            // Act
            var request = _fixture.Create<TrackProgressCommand>();
            var result = await _sut.Handle(request, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.InstanceOf(typeof(TrackProgressResponse)));
                Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test, MoqAutoData]
        public async Task TestFor404ResultWhenStartDateEqualsStopDate(
            [Frozen] Mock<IInternalApiClient<CommitmentsV2ApiConfiguration>> commitmentsV2Api)
        {
            // Arrange
            AddProviderResponse(ref commitmentsV2Api, HttpStatusCode.OK);

            var startdate = DateTime.Now;
            var apprenticeships = new List<ApprenticeshipDetailsResponse>()
            {
                _fixture.Build<ApprenticeshipDetailsResponse>().With(x => x.StartDate, startdate).With(x => x.StopDate, startdate).Create(),
                _fixture.Build<ApprenticeshipDetailsResponse>().With(x => x.StartDate, startdate).With(x => x.StopDate, startdate).Create(),
            };

            var apprenticeshipsResponse = _fixture.Build<GetApprenticeshipsResponse>()
                .With(x => x.StatusCode, HttpStatusCode.OK)
                .With(x => x.Apprenticeships, apprenticeships).Create();
            var apprenticeshipsApiResponse = new ApiResponse<GetApprenticeshipsResponse>(apprenticeshipsResponse, HttpStatusCode.OK, string.Empty);
            commitmentsV2Api.Setup(x => x.GetWithResponseCode<GetApprenticeshipsResponse>(It.IsAny<GetApprenticeshipsRequest>())).ReturnsAsync(apprenticeshipsApiResponse);

            _service = new CommitmentsV2Service(commitmentsV2Api.Object);
            _sut = new TrackProgressCommandHandler(_service);

            // Act
            var request = _fixture.Build<TrackProgressCommand>().With(x => x.PlannedStartDate, startdate).Create();
            var result = await _sut.Handle(request, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.InstanceOf(typeof(TrackProgressResponse)));
                Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test, MoqAutoData]
        public async Task TestFor400ResultWhenMultipleApprenticeshipsRemain(
            [Frozen] Mock<IInternalApiClient<CommitmentsV2ApiConfiguration>> commitmentsV2Api)
        {
            // Arrange
            AddProviderResponse(ref commitmentsV2Api, HttpStatusCode.OK);

            var startdate = DateTime.Now;
            var apprenticeships = new List<ApprenticeshipDetailsResponse>()
            {
                _fixture.Build<ApprenticeshipDetailsResponse>().With(x => x.StartDate, startdate).Create(),
                _fixture.Build<ApprenticeshipDetailsResponse>().With(x => x.StartDate, startdate).Create(),
            };

            var apprenticeshipsResponse = _fixture.Build<GetApprenticeshipsResponse>()
                .With(x => x.StatusCode, HttpStatusCode.OK)
                .With(x => x.Apprenticeships, apprenticeships).Create();
            var apprenticeshipsApiResponse = new ApiResponse<GetApprenticeshipsResponse>(apprenticeshipsResponse, HttpStatusCode.OK, string.Empty);
            commitmentsV2Api.Setup(x => x.GetWithResponseCode<GetApprenticeshipsResponse>(It.IsAny<GetApprenticeshipsRequest>())).ReturnsAsync(apprenticeshipsApiResponse);

            _service = new CommitmentsV2Service(commitmentsV2Api.Object);
            _sut = new TrackProgressCommandHandler(_service);

            // Act
            var request = _fixture.Build<TrackProgressCommand>().With(x => x.PlannedStartDate, startdate).Create();
            var result = await _sut.Handle(request, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.InstanceOf(typeof(TrackProgressResponse)));
                Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            });
        }

        private void AddProviderResponse(ref Mock<IInternalApiClient<CommitmentsV2ApiConfiguration>> client, HttpStatusCode statusCode)
        {
            var providerResponse = _fixture.Build<GetProviderResponse>().With(x => x.StatusCode, statusCode).Create();
            var providerApiResponse = new ApiResponse<GetProviderResponse>(providerResponse, statusCode, string.Empty);
            client.Setup(x => x.GetWithResponseCode<GetProviderResponse>(It.IsAny<GetProviderRequest>())).ReturnsAsync(providerApiResponse);
        }

        private void AddApprenticeshipResponse(ref Mock<IInternalApiClient<CommitmentsV2ApiConfiguration>> client, HttpStatusCode statusCode)
        {
            var apprenticeships = new List<ApprenticeshipDetailsResponse>()
            {
                _fixture.Create<ApprenticeshipDetailsResponse>()
            };

            var apprenticeshipsResponse = _fixture.Build<GetApprenticeshipsResponse>()
                .With(x => x.StatusCode, statusCode)
                .With(x => x.Apprenticeships, apprenticeships).Create();

            var apprenticeshipsApiResponse = new ApiResponse<GetApprenticeshipsResponse>(apprenticeshipsResponse, statusCode, string.Empty);
            client.Setup(x => x.GetWithResponseCode<GetApprenticeshipsResponse>(It.IsAny<GetApprenticeshipsRequest>())).ReturnsAsync(apprenticeshipsApiResponse);
        }
    }
}
