using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.Queries.GetVacanciesByAccountId;
using SFA.DAS.Recruit.Enums;
using System;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.EmployerAccounts
{
    [TestFixture]
    public class WhenGettingVacanciesByAccountId
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Account_From_Mediator(
            long accountId,
            int page,
            int pageSize,
            string sortColumn,
            string sortOrder,
            FilteringOptions filterBy,
            string searchTerm,
            GetVacanciesByAccountIdQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmployerAccountsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetVacanciesByAccountIdQuery>(c => c.AccountId.Equals(accountId) &&
                                                             c.Page.Equals(page) &&
                                                             c.PageSize.Equals(pageSize) && 
                                                             c.SortColumn.Equals(sortColumn) &&
                                                             c.SortOrder.Equals(sortOrder) &&
                                                             c.FilterBy.Equals(filterBy) &&
                                                             c.SearchTerm.Equals(searchTerm)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetVacancies(accountId, page, pageSize, sortColumn, sortOrder, filterBy, searchTerm) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetVacanciesByAccountIdQueryResult;
            Assert.That(model, Is.Not.Null);
            model.Should().BeEquivalentTo(mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            long accountId,
            int page,
            int pageSize,
            string sortColumn,
            string sortOrder,
            FilteringOptions filterBy,
            string searchTerm,
            GetVacanciesByAccountIdQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmployerAccountsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetVacanciesByAccountIdQuery>(c => c.AccountId.Equals(accountId) &&
                                                             c.Page.Equals(page) &&
                                                             c.PageSize.Equals(pageSize) &&
                                                             c.SortColumn.Equals(sortColumn) &&
                                                             c.SortOrder.Equals(sortOrder) &&
                                                             c.SearchTerm.Equals(searchTerm)),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetVacancies(accountId, page, pageSize, sortColumn, sortOrder, filterBy, searchTerm) as BadRequestResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}