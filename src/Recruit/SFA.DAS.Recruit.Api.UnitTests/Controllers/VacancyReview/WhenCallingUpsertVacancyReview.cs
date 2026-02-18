using System;
using System.Net;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Services;
using SFA.DAS.Recruit.Application.VacancyReview.Commands.UpsertVacancyReview;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.VacancyReview;

public class WhenCallingUpsertVacancyReview
{
    [Test, MoqAutoData]
    public async Task Then_Request_Is_Handled_And_Mediator_Command_Sent(
        Guid id,
        VacancyReviewDto vacancyReview,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VacancyReviewController sut)
    {
        // arrange
        sut.AddControllerContext();
        
        // act
        var actual = await sut.UpsertVacancyReview(id, vacancyReview, Mock.Of<IRecruitArtificialIntelligenceService>()) as CreatedResult;

        // assert
        actual.Should().NotBeNull();
        mediator.Verify(x => x.Send(
                It.Is<UpsertVacancyReviewCommand>(c => 
                    c.Id == id &&
                    c.VacancyReview.VacancyTitle == ((InnerApi.Recruit.Requests.VacancyReviewDto)vacancyReview).VacancyTitle), 
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_Exception_Thrown_InternalServerError_Returned(
        Guid id,
        VacancyReviewDto vacancyReview,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VacancyReviewController controller)
    {
        mediator.Setup(x => x.Send(
                It.Is<UpsertVacancyReviewCommand>(c => 
                    c.Id == id &&
                    c.VacancyReview.VacancyTitle == ((InnerApi.Recruit.Requests.VacancyReviewDto)vacancyReview).VacancyTitle), 
                It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
        
        var actual = await controller.UpsertVacancyReview(id, vacancyReview, Mock.Of<IRecruitArtificialIntelligenceService>()) as StatusCodeResult;

        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        
    }
}