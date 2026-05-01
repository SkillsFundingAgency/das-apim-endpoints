using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Api.Models.VacancyReviews;
using SFA.DAS.Recruit.Application.VacancyReview.Queries.GetVacancyReviewsByFilter;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.VacancyReview;

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
                VacancyReviews = new List<GetVacancyReviewResponse>()
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
