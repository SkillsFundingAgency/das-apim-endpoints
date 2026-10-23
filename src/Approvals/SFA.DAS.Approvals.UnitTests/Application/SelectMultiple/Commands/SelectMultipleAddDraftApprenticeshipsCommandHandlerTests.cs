using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Approvals.Application.SelectMultiple.Commands;
using SFA.DAS.Approvals.Application.SelectMultiple.Queries;
using SFA.DAS.Approvals.InnerApi.LearnerData;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.SelectMultiple.Commands;

public class SelectMultipleAddDraftApprenticeshipsCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Then_Throws_When_LearnerIds_Empty(
        SelectMultipleAddDraftApprenticeshipsCommand command,
        SelectMultipleAddDraftApprenticeshipsCommandHandler handler)
    {
        command.LearnerIds = [];

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("LearnerIds must not be empty*");
    }

    [Test, MoqAutoData]
    public async Task Then_Throws_When_ValidationErrors_Are_Returned(
        SelectMultipleAddDraftApprenticeshipsCommand command,
        LearnerDataValidationError validationError,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> learnerDataClient,
        SelectMultipleAddDraftApprenticeshipsCommandHandler handler)
    {
        command.LearnerIds = [1];

        mediator
            .Setup(x => x.Send(
                It.IsAny<ValidateSelectMultipleLearnerRecordsQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidateSelectMultipleLearnerRecordsQueryResult
            {
                ValidationErrors = [validationError]
            });

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Validation Failed for AddDraftApprenticeshipsCommand");

        learnerDataClient.Verify(
            x => x.PostWithResponseCode<List<LearnerDataRecord>>(It.IsAny<IPostApiRequest>(), true),
            Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Then_Throws_When_LearnerData_ErrorContent_Is_Set(
        SelectMultipleAddDraftApprenticeshipsCommand command,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> learnerDataClient,
        SelectMultipleAddDraftApprenticeshipsCommandHandler handler)
    {
        command.LearnerIds = [1];
        SetupValidationSuccess(mediator);

        learnerDataClient
            .Setup(x => x.PostWithResponseCode<List<LearnerDataRecord>>(
                It.IsAny<IPostApiRequest>(), true))
            .ReturnsAsync(new ApiResponse<List<LearnerDataRecord>>(
                null, HttpStatusCode.BadRequest, "learner missing"));

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Getting Learner Data Failed, Status Code BadRequest Error : learner missing");
    }

    [Test, MoqAutoData]
    public async Task Then_Throws_When_LearnerData_Count_Does_Not_Match(
        SelectMultipleAddDraftApprenticeshipsCommand command,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> learnerDataClient,
        SelectMultipleAddDraftApprenticeshipsCommandHandler handler)
    {
        var learner = Learner(1);
        command.LearnerIds = [learner.Id, learner.Id + 1];
        SetupValidationSuccess(mediator);

        learnerDataClient
            .Setup(x => x.PostWithResponseCode<List<LearnerDataRecord>>(
                It.IsAny<IPostApiRequest>(), true))
            .ReturnsAsync(new ApiResponse<List<LearnerDataRecord>>(
                [learner], HttpStatusCode.OK, string.Empty));

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Getting Learner Data Failed, expected 2 learners but received 1");
    }

    [Test, MoqAutoData]
    public async Task Then_Creates_Cohort_And_Posts_Mapped_Learners_When_All_Calls_Succeed(
        SelectMultipleAddDraftApprenticeshipsCommand command,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> learnerDataClient,
        [Frozen] Mock<IReservationApiClient<ReservationApiConfiguration>> reservationApiClient,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
        [Frozen] Mock<IAddCourseTypeFromRulesService> courseTypeService,
        Guid reservationId,
        SelectMultipleAddDraftApprenticeshipsCommandHandler handler)
    {
        var learner = Learner(1);
        command.LearnerIds = [learner.Id, learner.Id]; // Handler should deduplicate IDs.
        SetupValidationSuccess(mediator);

        learnerDataClient
            .Setup(x => x.PostWithResponseCode<List<LearnerDataRecord>>(
                It.IsAny<IPostApiRequest>(), true))
            .ReturnsAsync(new ApiResponse<List<LearnerDataRecord>>(
                [learner], HttpStatusCode.OK, string.Empty));

        reservationApiClient
            .Setup(x => x.PostWithResponseCode<BulkCreateReservationsWithNonLevyResult>(
                It.IsAny<IPostApiRequest>(), true))
            .ReturnsAsync(new ApiResponse<BulkCreateReservationsWithNonLevyResult>(
                new BulkCreateReservationsWithNonLevyResult
                {
                    BulkCreateResults =
                    [
                        new()
                        {
                            ULN = learner.Uln.ToString(),
                            ReservationId = reservationId
                        }
                    ]
                },
                HttpStatusCode.OK,
                string.Empty));

        var cohort = new CreateCohortResponse
        {
            CohortId = 123,
            CohortReference = "COHORT-123"
        };

        apiClient
            .Setup(x => x.PostWithResponseCode<CreateCohortResponse>(
                It.IsAny<IPostApiRequest>(), true))
            .ReturnsAsync(new ApiResponse<CreateCohortResponse>(
                cohort, HttpStatusCode.OK, string.Empty));

        apiClient
            .Setup(x => x.PostWithResponseCode<GetBulkUploadAddDraftApprenticeshipsResponse>(
                It.IsAny<IPostApiRequest>(), true))
            .ReturnsAsync(new ApiResponse<GetBulkUploadAddDraftApprenticeshipsResponse>(
                new GetBulkUploadAddDraftApprenticeshipsResponse(),
                HttpStatusCode.OK,
                string.Empty));

        var mappedLearners = new List<BulkUploadAddDraftApprenticeshipExtendedRequest>
        {
            new BulkUploadAddDraftApprenticeshipExtendedRequest()
            {
                Uln = learner.Uln.ToString(),
                FirstName = learner.FirstName,
                DateOfBirthAsString = "2000-01-02",
                CostAsString = "1500",
                CohortRef = cohort.CohortReference,
                ReservationId = reservationId,
                MinimumAgeAtApprenticeshipStart = 15,
                MaximumAgeAtApprenticeshipStart = 115
            }
        };
        courseTypeService
            .Setup(x => x.MapAndAddCourseTypeData(
                It.IsAny<List<BulkUploadAddDraftApprenticeshipRequest>>()))
            .ReturnsAsync(mappedLearners);

        var result = await handler.Handle(command, CancellationToken.None);

        result.CohortReference.Should().Be(cohort.CohortReference);

        courseTypeService.Verify(
            x => x.MapAndAddCourseTypeData(It.Is<List<BulkUploadAddDraftApprenticeshipRequest>>(
                records =>
                    records.Count == 1 &&
                    records[0].Uln == learner.Uln.ToString() &&
                    records[0].FirstName == learner.FirstName &&
                    records[0].DateOfBirthAsString == "2000-01-02" &&
                    records[0].CostAsString == "1500" &&
                    records[0].CohortRef == cohort.CohortReference &&
                    records[0].ReservationId == reservationId
                    )),
            Times.Once);

        apiClient.Verify(
            x => x.PostWithResponseCode<CreateCohortResponse>(It.IsAny<IPostApiRequest>(), true),
            Times.Once);

        apiClient.Verify(
            x => x.PostWithResponseCode<GetBulkUploadAddDraftApprenticeshipsResponse>(
                It.IsAny<IPostApiRequest>(), true),
            Times.Once);
    }

    private static void SetupValidationSuccess(Mock<IMediator> mediator)
    {
        mediator
            .Setup(x => x.Send(
                It.IsAny<ValidateSelectMultipleLearnerRecordsQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidateSelectMultipleLearnerRecordsQueryResult
            {
                ValidationErrors = []
            });
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