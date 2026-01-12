using System.Net;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RecruitQa.Api.Controllers;
using SFA.DAS.RecruitQa.Api.Models;
using SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetVacancyReviewsCountByUser;

namespace SFA.DAS.RecruitQa.Api.UnitTests.Controller.VacancyReview;

public class WhenCallingGetVacancyReviewsCountByUser
{
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Handled_And_Data_Returned(
        string userId,
        bool? approvedFirstTime,
        int count,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VacancyReviewController controller)
    {
        mediator.Setup(x => x.Send(
                It.Is<GetVacancyReviewsCountByUserQuery>(c => c.UserId == userId && c.ApprovedFirstTime == approvedFirstTime),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetVacancyReviewsCountByUserQueryResult { Count = count });

        var actual = await controller.GetCountByUser(userId, approvedFirstTime) as OkObjectResult;

        actual.Should().NotBeNull();
        var model = actual!.Value as GetVacancyReviewsCountApiResponse;
        model.Should().NotBeNull();
        model!.Count.Should().Be(count);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Exception_Internal_Server_Error_Returned(
        string userId,
        bool? approvedFirstTime,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VacancyReviewController controller)
    {
        mediator.Setup(x => x.Send(
                It.IsAny<GetVacancyReviewsCountByUserQuery>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var actual = await controller.GetCountByUser(userId, approvedFirstTime) as StatusCodeResult;

        actual.Should().NotBeNull();
        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
