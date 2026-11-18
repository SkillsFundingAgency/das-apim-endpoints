using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using SFA.DAS.RecruitQa.Application.VacancyReviews.Commands.UpsertVacancyReview;
using System.Net;
using IRecruitApiClient = SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>;


namespace SFA.DAS.RecruitQa.UnitTests.Application.Commands;

public class WhenHandlingUpsertVacancyReviewCommand
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Command_Is_Handled_And_Api_Called_For_Providers(
        UpsertVacancyReviewCommand command,
        [Frozen] Mock<IRecruitApiClient> recruitApiClient,
        [Greedy] UpsertVacancyReviewCommandHandler handler)
    {
        // arrange
        command.VacancyReview.ManualOutcome = "Approved";
        command.VacancyReview.OwnerType = OwnerType.Provider;
        command.VacancyReview.EmployerLocationOption = AvailableWhere.AcrossEngland;
        command.VacancyReview.Status = nameof(ReviewStatus.New);
        
        var expectedPutRequest = new PutVacancyreviewsByIdApiRequest
        {
            Id = command.Id,
            Data = new PutVacancyReviewRequest
            {
                AccountId = command.VacancyReview.AccountId,
                AccountLegalEntityId = command.VacancyReview.AccountLegalEntityId,
                VacancyReference = command.VacancyReview.VacancyReference.ToString(),
                Status = Enum.Parse<ReviewStatus>(command.VacancyReview.Status),
                ManualOutcome = command.VacancyReview.ManualOutcome,
                CreatedDate = command.VacancyReview.CreatedDate,
                OwnerType = command.VacancyReview.OwnerType,
                AutomatedQaOutcome = command.VacancyReview.AutomatedQaOutcome,
                ClosedDate = command.VacancyReview.ClosedDate,
                ManualQaComment = command.VacancyReview.ManualQaComment,
                ReviewedByUserEmail = command.VacancyReview.ReviewedByUserEmail,
                VacancyTitle = command.VacancyReview.VacancyTitle,
                ReviewedDate = command.VacancyReview.ReviewedDate,
                AutomatedQaOutcomeIndicators = command.VacancyReview.AutomatedQaOutcomeIndicators,
                DismissedAutomatedQaOutcomeIndicators = command.VacancyReview.DismissedAutomatedQaOutcomeIndicators,
                ManualQaEditFieldIndicators = command.VacancyReview.ManualQaEditFieldIndicators,
                ManualQaFieldIndicators = command.VacancyReview.ManualQaFieldIndicators,
                UpdatedFieldIdentifiers = command.VacancyReview.UpdatedFieldIdentifiers,
                SubmissionCount = command.VacancyReview.SubmissionCount,
                VacancySnapshot = command.VacancyReview.VacancySnapshot,
                Ukprn = command.VacancyReview.Ukprn,
                SlaDeadLine = command.VacancyReview.SlaDeadLine,
                SubmittedByUserEmail = command.VacancyReview.SubmittedByUserEmail,
            }
        };
        recruitApiClient
            .Setup(x => x.PutWithResponseCode<NullResponse>(It.IsAny<PutVacancyreviewsByIdApiRequest>()))
            .ReturnsAsync(new ApiResponse<NullResponse>(null!, HttpStatusCode.Created, ""));
        
        // act
        await handler.Handle(command, CancellationToken.None);

        // assert
        recruitApiClient.Verify(x => x.PutWithResponseCode<NullResponse>(It.Is<PutVacancyreviewsByIdApiRequest>(c => c.PutUrl == expectedPutRequest.PutUrl)), Times.Once);
    }
    
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Command_Is_An_Exception_Thrown_When_The_Call_Fails(
        UpsertVacancyReviewCommand command,
        [Frozen] Mock<IRecruitApiClient> recruitApiClient,
        UpsertVacancyReviewCommandHandler handler)
    {
        // arrange
        command.VacancyReview.Status = nameof(ReviewStatus.New);

        recruitApiClient
            .Setup(x => x.PutWithResponseCode<NullResponse>(It.IsAny<PutVacancyreviewsByIdApiRequest>()))
            .ReturnsAsync(new ApiResponse<NullResponse>(null!, HttpStatusCode.BadGateway, ""));
        
        // act
        var action = async () => await handler.Handle(command, CancellationToken.None);

        // assert
        await action.Should().ThrowAsync<ApiResponseException>();
    }
}