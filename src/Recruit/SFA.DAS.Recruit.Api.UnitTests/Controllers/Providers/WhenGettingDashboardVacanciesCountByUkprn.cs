using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.Queries.GetDashboardVacanciesCountByUkprn;
using SFA.DAS.Recruit.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Providers;
[TestFixture]
public class WhenGettingDashboardVacanciesCountByUkprn
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Account_From_Mediator(
        int ukprn,
        int pageNumber,
        int pageSize,
        string sortColumn,
        bool isAscending,
        List<ApplicationReviewStatus> status,
        GetDashboardVacanciesCountByUkprnQueryResult mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ProvidersController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetDashboardVacanciesCountByUkprnQuery>(c =>
                    c.Ukprn.Equals(ukprn) &&
                    c.PageNumber == pageNumber &&
                    c.PageSize == pageSize &&
                    c.SortColumn == sortColumn &&
                    c.IsAscending == isAscending && 
                    c.Status == status),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.GetDashboardVacanciesCount(ukprn, pageNumber, pageSize, sortColumn, isAscending, status) as ObjectResult;

        Assert.That(controllerResult, Is.Not.Null);
        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetDashboardVacanciesCountByUkprnQueryResult;
        Assert.That(model, Is.Not.Null);
        model.Should().BeEquivalentTo(mediatorResult);
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_Bad_Request(
        int ukprn,
        int pageNumber,
        int pageSize,
        string sortColumn,
        bool isAscending,
        List<ApplicationReviewStatus> status,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ProvidersController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetDashboardVacanciesCountByUkprnQuery>(c =>
                    c.Ukprn.Equals(ukprn) &&
                    c.PageNumber == pageNumber &&
                    c.PageSize == pageSize &&
                    c.SortColumn == sortColumn &&
                    c.IsAscending == isAscending &&
                    c.Status == status),
               It.IsAny<CancellationToken>()))
            .Throws<InvalidOperationException>();

        var controllerResult = await controller.GetDashboardVacanciesCount(ukprn, pageNumber, pageSize, sortColumn, isAscending, status) as BadRequestResult;

        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }
}
