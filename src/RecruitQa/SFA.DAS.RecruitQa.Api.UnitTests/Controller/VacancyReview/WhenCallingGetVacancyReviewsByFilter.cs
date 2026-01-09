using System.Net;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RecruitQa.Api.Controllers;
using SFA.DAS.RecruitQa.Api.Models;
using SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetVacancyReviewsByFilter;
using SFA.DAS.RecruitQa.InnerApi.Responses;

namespace SFA.DAS.RecruitQa.Api.UnitTests.Controller.VacancyReview;

public class WhenCallingGetVacancyReviewsByFilter
{
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Handled_And_Data_Returned(
        List<string> statuses,
        DateTime? expiredAssignationDateTime,
        List<GetVacancyReviewResponse> innerResponses,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VacancyReviewController controller)
    {
        mediator.Setup(x => x.Send(
                It.Is<GetVacancyReviewsByFilterQuery>(c => c.Status == statuses && c.ExpiredAssignationDateTime == expiredAssignationDateTime),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetVacancyReviewsByFilterQueryResult
            {
                VacancyReviews = innerResponses
            });

        var actual = await controller.Get(statuses, expiredAssignationDateTime) as OkObjectResult;

        actual.Should().NotBeNull();
        var model = actual!.Value as GetVacancyReviewsApiResponse;
        model.Should().NotBeNull();
        model!.VacancyReviews.Should().BeEquivalentTo(innerResponses.Select(r => (VacancyReviewDto)r).ToList());
    }

    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Handled_And_Empty_List_Returned_When_No_Data(
        List<string> statuses,
        DateTime? expiredAssignationDateTime,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VacancyReviewController controller)
    {
        mediator.Setup(x => x.Send(
                It.Is<GetVacancyReviewsByFilterQuery>(c => c.Status == statuses && c.ExpiredAssignationDateTime == expiredAssignationDateTime),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetVacancyReviewsByFilterQueryResult
            {
                VacancyReviews = new List<SFA.DAS.RecruitQa.InnerApi.Responses.GetVacancyReviewResponse>()
            });

        var actual = await controller.Get(statuses, expiredAssignationDateTime) as OkObjectResult;

        actual.Should().NotBeNull();
        var model = actual!.Value as GetVacancyReviewsApiResponse;
        model.Should().NotBeNull();
        model!.VacancyReviews.Should().BeEmpty();
    }

    [Test, MoqAutoData]
    public async Task Then_If_Exception_Internal_Server_Error_Returned(
        List<string> statuses,
        DateTime? expiredAssignationDateTime,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VacancyReviewController controller)
    {
        mediator.Setup(x => x.Send(
                It.IsAny<GetVacancyReviewsByFilterQuery>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var actual = await controller.Get(statuses, expiredAssignationDateTime) as StatusCodeResult;

        actual.Should().NotBeNull();
        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
