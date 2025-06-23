using System;
using System.Net;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.VacancyReview.Queries.GetVacancyReview;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.VacancyReview;

public class WhenCallingGetVacancyReviewById
{
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Handled_And_Data_Returned(
        Guid id,
        GetVacancyReviewQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VacancyReviewController controller)
    {
        mediator.Setup(x => x.Send(
                It.Is<GetVacancyReviewQuery>(c => c.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var actual = await controller.GetById(id) as OkObjectResult;
        
        actual.Should().NotBeNull();
        var actualModel = actual!.Value as GetVacancyReviewApiResponse;
        actualModel.Should().NotBeNull();
        actualModel!.VacancyReview.Should().BeEquivalentTo(queryResult.VacancyReview);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Handled_And_If_No_Data_Not_Found_Result_Returned(
        Guid id,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VacancyReviewController controller)
    {
        mediator.Setup(x => x.Send(
                It.Is<GetVacancyReviewQuery>(c => c.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetVacancyReviewQueryResult{VacancyReview = null});

        var actual = await controller.GetById(id) as NotFoundResult;

        actual.Should().NotBeNull();
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Handled_And_If_Exception_Internal_Server_Error_Returned(
        Guid id,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VacancyReviewController controller)
    {
        mediator.Setup(x => x.Send(
                It.Is<GetVacancyReviewQuery>(c => c.Id == id), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());
        
        var actual = await controller.GetById(id) as StatusCodeResult;

        actual.Should().NotBeNull();
        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}