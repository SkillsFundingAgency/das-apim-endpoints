using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.Queries.GetDashboardVacanciesCountByAccountId;
using SFA.DAS.Recruit.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.EmployerAccounts;
[TestFixture]
public class WhenGettingDashboardVacanciesCountByAccountId
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Account_From_Mediator(
        long accountId,
        int pageNumber,
        int pageSize,
        string sortColumn,
        bool isAscending,
        List<ApplicationReviewStatus> status,
        GetDashboardVacanciesCountByAccountIdQueryResult mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerAccountsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetDashboardVacanciesCountByAccountIdQuery>(c =>
                    c.AccountId.Equals(accountId) &&
                    c.PageNumber == pageNumber &&
                    c.PageSize == pageSize &&
                    c.SortColumn == sortColumn &&
                    c.IsAscending == isAscending && 
                    c.Status == status),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.GetDashboardVacanciesCount(accountId, pageNumber, pageSize, sortColumn, isAscending, status) as ObjectResult;

        Assert.That(controllerResult, Is.Not.Null);
        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetDashboardVacanciesCountByAccountIdQueryResult;
        Assert.That(model, Is.Not.Null);
        model.Should().BeEquivalentTo(mediatorResult);
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_Bad_Request(
        long accountId,
        int pageNumber,
        int pageSize,
        string sortColumn,
        bool isAscending,
        List<ApplicationReviewStatus> status,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerAccountsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetDashboardVacanciesCountByAccountIdQuery>(c =>
                    c.AccountId.Equals(accountId) &&
                    c.PageNumber == pageNumber &&
                    c.PageSize == pageSize &&
                    c.SortColumn == sortColumn &&
                    c.IsAscending == isAscending &&
                    c.Status == status),
               It.IsAny<CancellationToken>()))
            .Throws<InvalidOperationException>();

        var controllerResult = await controller.GetDashboardVacanciesCount(accountId, pageNumber, pageSize, sortColumn, isAscending, status) as BadRequestResult;

        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }
}
