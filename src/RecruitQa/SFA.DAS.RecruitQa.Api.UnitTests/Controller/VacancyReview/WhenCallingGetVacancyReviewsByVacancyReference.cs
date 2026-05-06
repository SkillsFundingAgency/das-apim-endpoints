using System.Net;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RecruitQa.Api.Controllers;
using SFA.DAS.RecruitQa.Api.Models;
using SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsByVacancyReference;
using SFA.DAS.RecruitQa.InnerApi.Responses;

namespace SFA.DAS.RecruitQa.Api.UnitTests.Controller.VacancyReview;

public class WhenCallingGetVacancyReviewsByVacancyReference
{
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Handled_And_Data_Returned(
        bool includeNoStatus,
        long vacancyReference,
        string status,
        List<string> manualOutcome,
        List<GetVacancyReviewResponse> innerResponses,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VacancyReviewController controller)
    {
        var fixture = new Fixture();
        innerResponses
            .Select(r => r.VacancyReference = $"VAC{fixture.Create<long>()}")
            .ToList();
        
        mediator.Setup(x => x.Send(
                It.Is<GetVacancyReviewsByVacancyReferenceQuery>(c => c.VacancyReference == vacancyReference && c.Status == status
                    && c.ManualOutcome == manualOutcome
                    && c.IncludeNoStatus == includeNoStatus),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetVacancyReviewsByVacancyReferenceQueryResult
            {
                VacancyReviews = innerResponses
            });

        var actual = await controller.GetByVacancyReference(vacancyReference, status, manualOutcome, includeNoStatus) as OkObjectResult;

        actual.Should().NotBeNull();
        var model = actual!.Value as GetVacancyReviewsApiResponse;
        model.Should().NotBeNull();
        model!.VacancyReviews.Should().BeEquivalentTo(innerResponses.Select(r => (VacancyReviewDto)r).ToList());
    }

    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Handled_And_Empty_List_Returned_When_No_Data(
        bool includeNoStatus,
        long vacancyReference,
        string status,
        List<string> manualOutcome,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VacancyReviewController controller)
    {
        mediator.Setup(x => x.Send(
                It.Is<GetVacancyReviewsByVacancyReferenceQuery>(c => c.VacancyReference == vacancyReference && c.Status == status && c.ManualOutcome == manualOutcome),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetVacancyReviewsByVacancyReferenceQueryResult
            {
                VacancyReviews = new List<GetVacancyReviewResponse>()
            });

        var actual = await controller.GetByVacancyReference(vacancyReference, status, manualOutcome, includeNoStatus) as OkObjectResult;

        actual.Should().NotBeNull();
        var model = actual!.Value as GetVacancyReviewsApiResponse;
        model.Should().NotBeNull();
        model!.VacancyReviews.Should().BeEmpty();
    }

    [Test, MoqAutoData]
    public async Task Then_If_Exception_Internal_Server_Error_Returned(
        bool includeNoStatus,
        long vacancyReference,
        string status,
        List<string> manualOutcome,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VacancyReviewController controller)
    {
        mediator.Setup(x => x.Send(
                It.IsAny<GetVacancyReviewsByVacancyReferenceQuery>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var actual = await controller.GetByVacancyReference(vacancyReference, status, manualOutcome, includeNoStatus) as StatusCodeResult;

        actual.Should().NotBeNull();
        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
