using System.Net;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RecruitQa.Api.Controllers;
using SFA.DAS.RecruitQa.Api.Models;
using SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsCountByAccountLegalEntity;

namespace SFA.DAS.RecruitQa.Api.UnitTests.Controller.VacancyReview;

public class WhenCallingGetVacancyReviewsCountByAccountLegalEntity
{
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Handled_And_Data_Returned(
        long accountLegalEntityId,
        string status,
        string manualOutcome,
        string employerNameOption,
        int count,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VacancyReviewController controller)
    {
        mediator.Setup(x => x.Send(
                It.Is<GetVacancyReviewsCountByAccountLegalEntityQuery>(c => 
                    c.AccountLegalEntityId == accountLegalEntityId
                    && c.Status == status
                    && c.ManualOutcome == manualOutcome
                    && c.EmployerNameOption == employerNameOption
                    ),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetVacancyReviewsCountByAccountLegalEntityQueryResult { Count = count });

        var actual = await controller.GetCountByAccountLegalEntity(accountLegalEntityId, status, manualOutcome, employerNameOption) as OkObjectResult;

        actual.Should().NotBeNull();
        var model = actual!.Value as GetVacancyReviewsCountApiResponse;
        model.Should().NotBeNull();
        model!.Count.Should().Be(count);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Exception_Internal_Server_Error_Returned(
        long accountLegalEntityId,
        string status,
        string manualOutcome,
        string employerNameOption,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VacancyReviewController controller)
    {
        mediator.Setup(x => x.Send(
                It.IsAny<GetVacancyReviewsCountByAccountLegalEntityQuery>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var actual = await controller.GetCountByAccountLegalEntity(accountLegalEntityId, status, manualOutcome, employerNameOption) as StatusCodeResult;

        actual.Should().NotBeNull();
        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
