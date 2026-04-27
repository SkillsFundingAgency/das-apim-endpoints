using System.Net;
using SFA.DAS.RecruitQa.Application.VacancyReviews.Commands.UpsertVacancyReview;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.RecruitQa.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;


using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Domain;
using SFA.DAS.SharedOuterApi.Types.Models;

namespace SFA.DAS.RecruitQa.UnitTests.Application.Commands;

public class WhenHandlingUpsertVacancyReviewCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Handled_And_Api_Called_For_Providers(
        UpsertVacancyReviewCommand command,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        UpsertVacancyReviewCommandHandler handler)
    {
        // arrange
        command.VacancyReview.ManualOutcome = "Approved";
        command.VacancyReview.OwnerType = "Provider";
        command.VacancyReview.EmployerLocationOption = AvailableWhere.AcrossEngland;
        
        var expectedPutRequest = new PutCreateVacancyReviewRequest(command.Id, command.VacancyReview);
        recruitApiClient
            .Setup(x => x.PutWithResponseCode<NullResponse>(It.Is<PutCreateVacancyReviewRequest>(c => c.PutUrl == expectedPutRequest.PutUrl)))
            .ReturnsAsync(new ApiResponse<NullResponse>(null!, HttpStatusCode.Created, ""));
        
        // act
        await handler.Handle(command, CancellationToken.None);

        // assert
        recruitApiClient.Verify(x => x.PutWithResponseCode<NullResponse>(It.Is<PutCreateVacancyReviewRequest>(c => c.PutUrl == expectedPutRequest.PutUrl)), Times.Once);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_An_Exception_Thrown_When_The_Call_Fails(
        UpsertVacancyReviewCommand command,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        UpsertVacancyReviewCommandHandler handler)
    {
        // arrange
        recruitApiClient
            .Setup(x => x.PutWithResponseCode<NullResponse>(It.IsAny<PutCreateVacancyReviewRequest>()))
            .ReturnsAsync(new ApiResponse<NullResponse>(null!, HttpStatusCode.BadGateway, ""));
        
        // act
        var action = async () => await handler.Handle(command, CancellationToken.None);

        // assert
        await action.Should().ThrowAsync<ApiResponseException>();
    }
}