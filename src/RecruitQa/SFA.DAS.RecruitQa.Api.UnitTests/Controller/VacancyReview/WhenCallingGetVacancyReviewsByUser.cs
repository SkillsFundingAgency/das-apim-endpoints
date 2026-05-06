using System.Net;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RecruitQa.Api.Controllers;
using SFA.DAS.RecruitQa.Api.Models;
using SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsByUser;
using SFA.DAS.RecruitQa.InnerApi.Responses;

namespace SFA.DAS.RecruitQa.Api.UnitTests.Controller.VacancyReview;

public class WhenCallingGetVacancyReviewsByUser
{
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Handled_And_Data_Returned(
        string userId,
        string status,
        DateTime? assignationExpiry,
        List<GetVacancyReviewResponse> innerResponses,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VacancyReviewController controller)
    {
        var fixture = new Fixture();
        innerResponses
            .Select(r => r.VacancyReference = $"VAC{fixture.Create<long>()}")
            .ToList();
        mediator.Setup(x => x.Send(
                It.Is<GetVacancyReviewsByUserQuery>(c => c.UserId == userId && c.AssignationExpiry == assignationExpiry),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetVacancyReviewsByUserQueryResult
            {
                VacancyReviews = innerResponses
            });

        var actual = await controller.GetByUser(userId, assignationExpiry, status) as OkObjectResult;

        actual.Should().NotBeNull();
        var model = actual!.Value as GetVacancyReviewsApiResponse;
        model.Should().NotBeNull();
        model!.VacancyReviews.Should().BeEquivalentTo(innerResponses.Select(r => (VacancyReviewDto)r).ToList());
    }

    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Handled_And_Empty_List_Returned_When_No_Data(
        string userId,
        DateTime? assignationExpiry,
        string status,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VacancyReviewController controller)
    {
        mediator.Setup(x => x.Send(
                It.Is<GetVacancyReviewsByUserQuery>(c => c.UserId == userId 
                                                         && c.AssignationExpiry == assignationExpiry
                                                         && c.Status == status),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetVacancyReviewsByUserQueryResult
            {
                VacancyReviews = new List<GetVacancyReviewResponse>()
            });

        var actual = await controller.GetByUser(userId, assignationExpiry, status) as OkObjectResult;

        actual.Should().NotBeNull();
        var model = actual!.Value as GetVacancyReviewsApiResponse;
        model.Should().NotBeNull();
        model!.VacancyReviews.Should().BeEmpty();
    }

    [Test, MoqAutoData]
    public async Task Then_If_Exception_Internal_Server_Error_Returned(
        string userId,
        string status,
        DateTime? assignationExpiry,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VacancyReviewController controller)
    {
        mediator.Setup(x => x.Send(
                It.IsAny<GetVacancyReviewsByUserQuery>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var actual = await controller.GetByUser(userId, assignationExpiry, status) as StatusCodeResult;

        actual.Should().NotBeNull();
        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
