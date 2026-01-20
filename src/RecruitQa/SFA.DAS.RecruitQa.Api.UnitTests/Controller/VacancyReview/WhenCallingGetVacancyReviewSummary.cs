using System.Net;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RecruitQa.Api.Controllers;
using SFA.DAS.RecruitQa.Api.Models;
using SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewSummary;

namespace SFA.DAS.RecruitQa.Api.UnitTests.Controller.VacancyReview;

public class WhenCallingGetVacancyReviewSummary
{
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Handled_And_Data_Returned(
        GetVacancyReviewSummaryQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VacancyReviewController controller)
    {
        mediator.Setup(x => x.Send(
                It.IsAny<GetVacancyReviewSummaryQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var actual = await controller.GetSummary() as OkObjectResult;

        actual.Should().NotBeNull();
        var model = actual!.Value as GetVacancyReviewSummaryApiResponse;
        model.Should().NotBeNull();
        model!.TotalVacanciesForReview.Should().Be(queryResult.TotalVacanciesForReview);
        model.TotalVacanciesResubmitted.Should().Be(queryResult.TotalVacanciesResubmitted);
        model.TotalVacanciesBrokenSla.Should().Be(queryResult.TotalVacanciesBrokenSla);
        model.TotalVacanciesSubmittedTwelveTwentyFourHours.Should().Be(queryResult.TotalVacanciesSubmittedTwelveTwentyFourHours);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Exception_Internal_Server_Error_Returned(
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VacancyReviewController controller)
    {
        mediator.Setup(x => x.Send(
                It.IsAny<GetVacancyReviewSummaryQuery>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var actual = await controller.GetSummary() as StatusCodeResult;

        actual.Should().NotBeNull();
        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
