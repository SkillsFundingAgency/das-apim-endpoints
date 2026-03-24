using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetAssignAllowEmployerAdd;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Requests;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Responses;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.Approvals.Types;
using SFA.DAS.SharedOuterApi.Common;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Reservations;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.Cohorts.Queries;

[TestFixture]
public class GetAssignAllowEmployerAddQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_Returns_AllowEmployerAdd_From_CourseTypes_When_Reservation_Has_LearningType(
        GetAssignAllowEmployerAddQuery query,
        [Frozen] Mock<IReservationApiClient<ReservationApiConfiguration>> reservationApiClient,
        [Frozen] Mock<ICourseTypesApiClient> courseTypesApiClient,
        GetAssignAllowEmployerAddQueryHandler handler)
    {
        var reservationResponse = new GetReservationResponse
        {
            Id = query.ReservationId,
            Course = new ReservationCourseResponse { LearningType = LearningType.ApprenticeshipUnit }
        };
        var allowEmployerAddResponse = new GetAllowEmployerAddResponse { AllowEmployerAdd = false };

        reservationApiClient
            .Setup(x => x.Get<GetReservationResponse>(It.Is<GetReservationRequest>(r => r.Id == query.ReservationId)))
            .ReturnsAsync(reservationResponse);
        courseTypesApiClient
            .Setup(x => x.Get<GetAllowEmployerAddResponse>(It.Is<GetAllowEmployerAddRequest>(r => r.LearningType == "ApprenticeshipUnit")))
            .ReturnsAsync(allowEmployerAddResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result!.LearningType.Should().Be(2);
        result.AllowEmployerAdd.Should().BeFalse();
    }

    [Test, MoqAutoData]
    public async Task Handle_Returns_AllowEmployerAdd_True_When_LearningType_Is_Apprenticeship(
        GetAssignAllowEmployerAddQuery query,
        [Frozen] Mock<IReservationApiClient<ReservationApiConfiguration>> reservationApiClient,
        [Frozen] Mock<ICourseTypesApiClient> courseTypesApiClient,
        GetAssignAllowEmployerAddQueryHandler handler)
    {
        var reservationResponse = new GetReservationResponse
        {
            Id = query.ReservationId,
            Course = new ReservationCourseResponse { LearningType = LearningType.Apprenticeship }
        };
        var allowEmployerAddResponse = new GetAllowEmployerAddResponse { AllowEmployerAdd = true };

        reservationApiClient
            .Setup(x => x.Get<GetReservationResponse>(It.Is<GetReservationRequest>(r => r.Id == query.ReservationId)))
            .ReturnsAsync(reservationResponse);
        courseTypesApiClient
            .Setup(x => x.Get<GetAllowEmployerAddResponse>(It.Is<GetAllowEmployerAddRequest>(r => r.LearningType == "Apprenticeship")))
            .ReturnsAsync(allowEmployerAddResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result!.LearningType.Should().Be(0);
        result.AllowEmployerAdd.Should().BeTrue();
    }

    [Test, MoqAutoData]
    public async Task Handle_Returns_Null_When_Reservation_Is_Null(
        GetAssignAllowEmployerAddQuery query,
        [Frozen] Mock<IReservationApiClient<ReservationApiConfiguration>> reservationApiClient,
        [Frozen] Mock<ICourseTypesApiClient> courseTypesApiClient,
        GetAssignAllowEmployerAddQueryHandler handler)
    {
        reservationApiClient
            .Setup(x => x.Get<GetReservationResponse>(It.Is<GetReservationRequest>(r => r.Id == query.ReservationId)))
            .ReturnsAsync((GetReservationResponse)null);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeNull();
        courseTypesApiClient.Verify(
            x => x.Get<GetAllowEmployerAddResponse>(It.IsAny<GetAllowEmployerAddRequest>()),
            Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Handle_Returns_Null_When_Reservation_Course_Is_Null(
        GetAssignAllowEmployerAddQuery query,
        [Frozen] Mock<IReservationApiClient<ReservationApiConfiguration>> reservationApiClient,
        [Frozen] Mock<ICourseTypesApiClient> courseTypesApiClient,
        GetAssignAllowEmployerAddQueryHandler handler)
    {
        var reservationResponse = new GetReservationResponse { Id = query.ReservationId, Course = null };
        reservationApiClient
            .Setup(x => x.Get<GetReservationResponse>(It.Is<GetReservationRequest>(r => r.Id == query.ReservationId)))
            .ReturnsAsync(reservationResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeNull();
        courseTypesApiClient.Verify(
            x => x.Get<GetAllowEmployerAddResponse>(It.IsAny<GetAllowEmployerAddRequest>()),
            Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Handle_Calls_CourseTypes_With_Null_LearningType_String_When_Reservation_LearningType_Is_Unknown(
        GetAssignAllowEmployerAddQuery query,
        [Frozen] Mock<IReservationApiClient<ReservationApiConfiguration>> reservationApiClient,
        [Frozen] Mock<ICourseTypesApiClient> courseTypesApiClient,
        GetAssignAllowEmployerAddQueryHandler handler)
    {
        var reservationResponse = new GetReservationResponse
        {
            Id = query.ReservationId,
            Course = new ReservationCourseResponse { LearningType = (LearningType)99 }
        };
        reservationApiClient
            .Setup(x => x.Get<GetReservationResponse>(It.Is<GetReservationRequest>(r => r.Id == query.ReservationId)))
            .ReturnsAsync(reservationResponse);
        courseTypesApiClient
            .Setup(x => x.Get<GetAllowEmployerAddResponse>(It.Is<GetAllowEmployerAddRequest>(r => r.LearningType == null)))
            .ReturnsAsync(new GetAllowEmployerAddResponse { AllowEmployerAdd = true });

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result!.LearningType.Should().Be(99);
        result.AllowEmployerAdd.Should().BeTrue();
        courseTypesApiClient.Verify(
            x => x.Get<GetAllowEmployerAddResponse>(It.Is<GetAllowEmployerAddRequest>(r => r.LearningType == null)),
            Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_Returns_AllowEmployerAdd_True_And_Null_LearningType_When_Reservation_Course_LearningType_Is_Null(
        GetAssignAllowEmployerAddQuery query,
        [Frozen] Mock<IReservationApiClient<ReservationApiConfiguration>> reservationApiClient,
        [Frozen] Mock<ICourseTypesApiClient> courseTypesApiClient,
        GetAssignAllowEmployerAddQueryHandler handler)
    {
        var reservationResponse = new GetReservationResponse
        {
            Id = query.ReservationId,
            Course = new ReservationCourseResponse { LearningType = null }
        };
        reservationApiClient
            .Setup(x => x.Get<GetReservationResponse>(It.Is<GetReservationRequest>(r => r.Id == query.ReservationId)))
            .ReturnsAsync(reservationResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result!.LearningType.Should().BeNull();
        result.AllowEmployerAdd.Should().BeTrue();
        courseTypesApiClient.Verify(
            x => x.Get<GetAllowEmployerAddResponse>(It.IsAny<GetAllowEmployerAddRequest>()),
            Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Handle_Calls_ReservationApi_With_Correct_Request(
        GetAssignAllowEmployerAddQuery query,
        [Frozen] Mock<IReservationApiClient<ReservationApiConfiguration>> reservationApiClient,
        [Frozen] Mock<ICourseTypesApiClient> courseTypesApiClient,
        GetAssignAllowEmployerAddQueryHandler handler)
    {
        var reservationResponse = new GetReservationResponse
        {
            Id = query.ReservationId,
            Course = new ReservationCourseResponse { LearningType = LearningType.FoundationApprenticeship }
        };
        reservationApiClient
            .Setup(x => x.Get<GetReservationResponse>(It.IsAny<GetReservationRequest>()))
            .ReturnsAsync(reservationResponse);
        courseTypesApiClient
            .Setup(x => x.Get<GetAllowEmployerAddResponse>(It.IsAny<GetAllowEmployerAddRequest>()))
            .ReturnsAsync(new GetAllowEmployerAddResponse { AllowEmployerAdd = true });

        await handler.Handle(query, CancellationToken.None);

        reservationApiClient.Verify(
            x => x.Get<GetReservationResponse>(It.Is<GetReservationRequest>(r => r.Id == query.ReservationId)),
            Times.Once);
    }
}
