using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Application.Learners.Queries;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.SelectLearners;

public class WhenGettingLearnersForProvider
{
    [Test, MoqAutoData]
    public async Task Then_Get_Returns_Learners_From_Mediator(
        long providerId,
        long? accountLegalEntityId,
        long? cohortId,
        string searchTerm,
        string sortColumn,
        bool sortDescending,
        int page,
        int? pageSize,
        int? startMonth,
        int startYear,
        int courseCode,
        GetLearnersForProviderQueryResult mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] SelectLearnersController controller)
    {
        mockMediator.Setup(mediator => mediator.Send(
                It.IsAny<GetLearnersForProviderQuery>(),
                It.IsAny<CancellationToken>()))
            .Callback((object o, CancellationToken ct) =>
            {
                var q = (GetLearnersForProviderQuery)o;
                q.ProviderId.Should().Be(providerId);
                q.StartYear.Should().Be(startYear);
                q.StartMonth.Should().Be(startMonth);
                q.CourseCode.Should().Be(courseCode);
                q.AccountLegalEntityId.Should().Be(accountLegalEntityId);
                q.SearchTerm.Should().Be(searchTerm);
                q.SortField.Should().Be(sortColumn);
                q.SortDescending.Should().Be(sortDescending);
                q.Page.Should().Be(page);
            })
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.Get(providerId, accountLegalEntityId, cohortId, searchTerm, sortColumn, sortDescending, page, pageSize, startMonth, startYear,courseCode.ToString()) as ObjectResult;

        controllerResult.Should().NotBeNull();
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetLearnersForProviderQueryResult;
        model.Should().NotBeNull();
        model.Should().BeEquivalentTo(mediatorResult);
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_InternalServerError(
        long providerId,
        long? accountLegalEntityId,
        long? cohortId,
        string searchTerm,
        string sortColumn,
        bool sortDescending,
        int page,
        int pagesize,
        int? startMonth,
        int startYear,
        string courseCode,
        GetLearnersForProviderQueryResult mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] SelectLearnersController controller)
    {
        mockMediator.Setup(mediator => mediator.Send(
                It.IsAny<GetLearnersForProviderQuery>(),
                It.IsAny<CancellationToken>()))
            .Throws<InvalidOperationException>();

        var controllerResult = await controller.Get(providerId, accountLegalEntityId, cohortId, searchTerm, sortColumn, sortDescending, page, pagesize, startMonth, startYear, courseCode) as StatusCodeResult;

        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}