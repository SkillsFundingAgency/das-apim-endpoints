using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Approvals.Application.SelectMultiple.Queries;
using SFA.DAS.Approvals.InnerApi.LearnerData;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.Approvals.Types;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.SelectMultiple.Queries;

public class ValidateSelectMultipleLearnerRecordsQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Then_Throws_When_LearnerIds_Empty(
        ValidateSelectMultipleLearnerRecordsQuery query,
        ValidateSelectMultipleLearnerRecordsQueryHandler handler)
    {
        query.LearnerIds = [];

        var act = () => handler.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("LearnerIds must not be empty*");
    }

    [Test, MoqAutoData]
    public async Task Then_Throws_When_LearnerData_ErrorContent_Is_Set(
        ValidateSelectMultipleLearnerRecordsQuery query,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> learnerDataClient,
        [Frozen] Mock<IReservationApiClient<ReservationApiConfiguration>> reservationApiClient,
        ValidateSelectMultipleLearnerRecordsQueryHandler handler)
    {
        var learner = Learner(1);
        query.LearnerIds = [learner.Id];
        learnerDataClient
            .Setup(x => x.PostWithResponseCode<List<LearnerDataRecord>>(It.IsAny<IPostApiRequest>(), true))
            .ReturnsAsync(new ApiResponse<List<LearnerDataRecord>>(null, HttpStatusCode.BadRequest, "learner missing"));

        var act = () => handler.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Getting Learner Data Failed, Status Code BadRequest Error : learner missing");
        reservationApiClient.Verify(
            x => x.PostWithResponseCode<BulkReservationValidationResults>(It.IsAny<IPostApiRequest>(), true),
            Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Then_Throws_When_LearnerData_Count_Does_Not_Match(
        ValidateSelectMultipleLearnerRecordsQuery query,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> learnerDataClient,
        ValidateSelectMultipleLearnerRecordsQueryHandler handler)
    {
        var learner = Learner(1);
        query.LearnerIds = [learner.Id, learner.Id + 1];
        learnerDataClient
            .Setup(x => x.PostWithResponseCode<List<LearnerDataRecord>>(It.IsAny<IPostApiRequest>(), true))
            .ReturnsAsync(new ApiResponse<List<LearnerDataRecord>>([learner], HttpStatusCode.OK, string.Empty));

        var act = () => handler.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Getting Learner Data Failed, expected 2 learners but received 1");
    }

    [Test, MoqAutoData]
    public async Task Then_Throws_When_Reservation_ErrorContent_Is_Set(
        ValidateSelectMultipleLearnerRecordsQuery query,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> learnerDataClient,
        [Frozen] Mock<IReservationApiClient<ReservationApiConfiguration>> reservationApiClient,
        [Frozen] Mock<IProviderCoursesOrStandardsService> providerCoursesService,
        ValidateSelectMultipleLearnerRecordsQueryHandler handler)
    {
        var learner = Learner(1);
        query.LearnerIds = [learner.Id];
        SetupLearnerData(learnerDataClient, learner);
        reservationApiClient
            .Setup(x => x.PostWithResponseCode<BulkReservationValidationResults>(It.IsAny<IPostApiRequest>(), true))
            .ReturnsAsync(new ApiResponse<BulkReservationValidationResults>(null, HttpStatusCode.BadRequest, "no reservation"));

        var act = () => handler.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Validating Reservations Failed, Status Code BadRequest Error : no reservation");
        providerCoursesService.Verify(x => x.GetCoursesData(It.IsAny<long>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Then_Throws_When_Commitments_ErrorContent_Is_Set(
        ValidateSelectMultipleLearnerRecordsQuery query,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> learnerDataClient,
        [Frozen] Mock<IReservationApiClient<ReservationApiConfiguration>> reservationApiClient,
        [Frozen] Mock<IProviderCoursesOrStandardsService> providerCoursesService,
        [Frozen] Mock<IBulkCourseMetadataService> bulkCourseMetadataService,
        [Frozen] Mock<IAddCourseTypeDataToCsvService> courseTypesToCsvService,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
        List<BulkUploadAddDraftApprenticeshipExtendedRequest> csvRecords,
        ValidateSelectMultipleLearnerRecordsQueryHandler handler)
    {
        var learner = Learner(1);
        query.LearnerIds = [learner.Id];
        SetupLearnerData(learnerDataClient, learner);
        SetupReservation(reservationApiClient);
        SetupCourseData(providerCoursesService, bulkCourseMetadataService, courseTypesToCsvService, query.ProviderId, csvRecords);
        apiClient
            .Setup(x => x.PostWithResponseCode<PostValidateSelectMultipleLearnersResponse>(It.IsAny<IPostApiRequest>(), true))
            .ReturnsAsync(new ApiResponse<PostValidateSelectMultipleLearnersResponse>(null, HttpStatusCode.BadRequest, "commitments failed"));

        var act = () => handler.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Validating Select Multiple Learners Failed, Status Code BadRequest Error : commitments failed");
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_ValidationErrors_When_All_Calls_Succeed(
        ValidateSelectMultipleLearnerRecordsQuery query,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> learnerDataClient,
        [Frozen] Mock<IReservationApiClient<ReservationApiConfiguration>> reservationApiClient,
        [Frozen] Mock<IProviderCoursesOrStandardsService> providerCoursesService,
        [Frozen] Mock<IBulkCourseMetadataService> bulkCourseMetadataService,
        [Frozen] Mock<IAddCourseTypeDataToCsvService> courseTypesToCsvService,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
        List<BulkUploadAddDraftApprenticeshipExtendedRequest> csvRecords,
        List<LearnerDataValidationError> validationErrors,
        ValidateSelectMultipleLearnerRecordsQueryHandler handler)
    {
        var learner = Learner(1);
        query.LearnerIds = [learner.Id];
        SetupLearnerData(learnerDataClient, learner);
        SetupReservation(reservationApiClient);
        SetupCourseData(providerCoursesService, bulkCourseMetadataService, courseTypesToCsvService, query.ProviderId, csvRecords);
        apiClient
            .Setup(x => x.PostWithResponseCode<PostValidateSelectMultipleLearnersResponse>(It.IsAny<IPostApiRequest>(), true))
            .ReturnsAsync(new ApiResponse<PostValidateSelectMultipleLearnersResponse>(
                new PostValidateSelectMultipleLearnersResponse { ValidationErrors = validationErrors },
                HttpStatusCode.OK,
                string.Empty));

        var result = await handler.Handle(query, CancellationToken.None);

        result.ValidationErrors.Should().BeEquivalentTo(validationErrors);
    }

    private static void SetupLearnerData(
        Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> learnerDataClient,
        LearnerDataRecord learner)
    {
        learnerDataClient
            .Setup(x => x.PostWithResponseCode<List<LearnerDataRecord>>(It.IsAny<IPostApiRequest>(), true))
            .ReturnsAsync(new ApiResponse<List<LearnerDataRecord>>([learner], HttpStatusCode.OK, string.Empty));
    }

    private static void SetupReservation(Mock<IReservationApiClient<ReservationApiConfiguration>> reservationApiClient)
    {
        reservationApiClient
            .Setup(x => x.PostWithResponseCode<BulkReservationValidationResults>(It.IsAny<IPostApiRequest>(), true))
            .ReturnsAsync(new ApiResponse<BulkReservationValidationResults>(
                new BulkReservationValidationResults(),
                HttpStatusCode.OK,
                string.Empty));
    }

    private static void SetupCourseData(
        Mock<IProviderCoursesOrStandardsService> providerCoursesService,
        Mock<IBulkCourseMetadataService> bulkCourseMetadataService,
        Mock<IAddCourseTypeDataToCsvService> courseTypesToCsvService,
        long providerId,
        List<BulkUploadAddDraftApprenticeshipExtendedRequest> csvRecords)
    {
        providerCoursesService
            .Setup(x => x.GetCoursesData(providerId))
            .ReturnsAsync(new ProviderStandardsData { IsMainProvider = true });
        bulkCourseMetadataService
            .Setup(x => x.GetOtjTrainingHoursForBulkUploadAsync(It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(new Dictionary<string, int?>());
        courseTypesToCsvService
            .Setup(x => x.MapAndAddCourseTypeData(It.IsAny<List<BulkUploadAddDraftApprenticeshipRequest>>()))
            .ReturnsAsync(csvRecords);
    }

    private static LearnerDataRecord Learner(long id) => new()
    {
        Id = id,
        Uln = 1000000000 + id,
        FirstName = "Ada",
        LastName = "Lovelace",
        Email = "ada@example.com",
        Dob = new DateTime(2000, 1, 2),
        StartDate = new DateTime(2024, 9, 1),
        PlannedEndDate = new DateTime(2026, 8, 31),
        TrainingCode = "123",
        TrainingPrice = 1000,
        EpaoPrice = 500
    };
}
