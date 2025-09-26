using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.Queries.GetVacanciesByUkprn;
using SFA.DAS.Recruit.Enums;
using System;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Providers
{
    [TestFixture]
    public class WhenGettingVacanciesByUkprn
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Account_From_Mediator(
            int ukprn,
            string userId,
            int page,
            int pageSize,
            string sortColumn,
            string sortOrder,
            FilteringOptions filterBy,
            string searchTerm,
            GetVacanciesByUkprnQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProvidersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetVacanciesByUkprnQuery>(c => c.Ukprn.Equals(ukprn) &&
                                                             c.UserId.Equals(userId) &&
                                                             c.Page.Equals(page) &&
                                                             c.PageSize.Equals(pageSize) && 
                                                             c.SortColumn.Equals(sortColumn) &&
                                                             c.SortOrder.Equals(sortOrder) &&
                                                             c.FilterBy.Equals(filterBy) &&
                                                             c.SearchTerm.Equals(searchTerm)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetVacancies(ukprn, userId, page, pageSize, sortColumn, sortOrder, filterBy, searchTerm) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetVacanciesByUkprnQueryResult;
            Assert.That(model, Is.Not.Null);
            model.Should().BeEquivalentTo(mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            int ukprn,
            string userId,
            int page,
            int pageSize,
            string sortColumn,
            string sortOrder,
            FilteringOptions filterBy,
            string searchTerm,
            GetVacanciesByUkprnQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProvidersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetVacanciesByUkprnQuery>(c => c.Ukprn.Equals(ukprn) &&
                                                             c.UserId.Equals(userId) &&
                                                             c.Page.Equals(page) &&
                                                             c.PageSize.Equals(pageSize) &&
                                                             c.SortColumn.Equals(sortColumn) &&
                                                             c.SortOrder.Equals(sortOrder) &&
                                                             c.SearchTerm.Equals(searchTerm)),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetVacancies(ukprn, userId, page, pageSize, sortColumn, sortOrder, filterBy, searchTerm) as BadRequestResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
