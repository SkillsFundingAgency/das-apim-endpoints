using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using SFA.DAS.RecruitQa.Api.Controllers;
using SFA.DAS.RecruitQa.Api.Models;
using SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsByFilter;
using System.Net;

namespace SFA.DAS.RecruitQa.Api.UnitTests.Controller.VacancyReview;

public class WhenCallingGetVacancyReviewsByFilter
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Handled_And_Data_Returned(
        List<ReviewStatus> statuses,
        DateTime? expiredAssignationDateTime,
        List<Recruit.Contracts.ApiResponses.VacancyReview> innerResponses,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VacancyReviewController controller)
    {
        var fixture = new Fixture();
        innerResponses
            .Select(r => r.VacancyReference = $"VAC{fixture.Create<long>()}")
            .ToList();
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
        model!.VacancyReviews.Should().BeEquivalentTo([.. innerResponses.Select(r => (VacancyReviewDto) r)]);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Handled_And_Empty_List_Returned_When_No_Data(
        List<ReviewStatus> statuses,
        DateTime? expiredAssignationDateTime,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VacancyReviewController controller)
    {
        mediator.Setup(x => x.Send(
                It.Is<GetVacancyReviewsByFilterQuery>(c => c.Status == statuses && c.ExpiredAssignationDateTime == expiredAssignationDateTime),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetVacancyReviewsByFilterQueryResult
            {
                VacancyReviews = []
            });

        var actual = await controller.Get(statuses, expiredAssignationDateTime) as OkObjectResult;

        actual.Should().NotBeNull();
        var model = actual!.Value as GetVacancyReviewsApiResponse;
        model.Should().NotBeNull();
        model!.VacancyReviews.Should().BeEmpty();
    }

    [Test, MoqAutoData]
    public async Task Then_If_Exception_Internal_Server_Error_Returned(
        List<ReviewStatus> statuses,
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