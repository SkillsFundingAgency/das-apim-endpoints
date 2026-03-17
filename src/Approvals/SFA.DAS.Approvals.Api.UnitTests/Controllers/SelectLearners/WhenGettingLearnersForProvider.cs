using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.Application.Learners.Queries;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.SelectLearners;

public class WhenGettingLearnersForProvider
{
    [Test, MoqAutoData]
    public async Task Then_Get_Returns_Learners_From_Mediator(
        long providerId,
        SearchLearnersRequest request,
        GetLearnersForProviderQueryResult mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] SelectLearnersController controller)
    {
        request.CourseCode = "1";
        mockMediator.Setup(mediator => mediator.Send(
                It.IsAny<GetLearnersForProviderQuery>(),
                It.IsAny<CancellationToken>()))
            .Callback((object o, CancellationToken ct) =>
            {
                var q = (GetLearnersForProviderQuery)o;
                q.ProviderId.Should().Be(providerId);
                q.StartYear.Should().Be(request.StartYear);
                q.StartMonth.Should().Be(request.StartMonth);
                q.CourseCode.Should().Be(int.Parse(request.CourseCode));
                q.AccountLegalEntityId.Should().Be(request.AccountLegalEntityId);
                q.SearchTerm.Should().Be(request.SearchTerm);
                q.SortField.Should().Be(request.SortColumn);
                q.SortDescending.Should().Be(request.SortDescending);
                q.Page.Should().Be(request.Page);
            })
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.Get(providerId,request) as ObjectResult;

        controllerResult.Should().NotBeNull();
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetLearnersForProviderQueryResult;
        model.Should().NotBeNull();
        model.Should().BeEquivalentTo(mediatorResult);
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_InternalServerError(
        long providerId,
        SearchLearnersRequest request,
        GetLearnersForProviderQueryResult mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] SelectLearnersController controller)
    {
        mockMediator.Setup(mediator => mediator.Send(
                It.IsAny<GetLearnersForProviderQuery>(),
                It.IsAny<CancellationToken>()))
            .Throws<InvalidOperationException>();

        var controllerResult = await controller.Get(providerId,request) as StatusCodeResult;

        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}