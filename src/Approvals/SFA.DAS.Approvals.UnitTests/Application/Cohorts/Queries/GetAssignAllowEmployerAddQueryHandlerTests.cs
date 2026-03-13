using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetAssignAllowEmployerAdd;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Requests;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Responses;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
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
            Course = new ReservationCourseResponse { LearningType = 2 }
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
            Course = new ReservationCourseResponse { LearningType = 0 }
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
    public async Task Handle_Returns_AllowEmployerAdd_True_When_LearningType_Is_Unknown(
        GetAssignAllowEmployerAddQuery query,
        [Frozen] Mock<IReservationApiClient<ReservationApiConfiguration>> reservationApiClient,
        [Frozen] Mock<ICourseTypesApiClient> courseTypesApiClient,
        GetAssignAllowEmployerAddQueryHandler handler)
    {
        var reservationResponse = new GetReservationResponse
        {
            Id = query.ReservationId,
            Course = new ReservationCourseResponse { LearningType = 99 }
        };
        reservationApiClient
            .Setup(x => x.Get<GetReservationResponse>(It.Is<GetReservationRequest>(r => r.Id == query.ReservationId)))
            .ReturnsAsync(reservationResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result!.LearningType.Should().Be(99);
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
            Course = new ReservationCourseResponse { LearningType = 1 }
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
