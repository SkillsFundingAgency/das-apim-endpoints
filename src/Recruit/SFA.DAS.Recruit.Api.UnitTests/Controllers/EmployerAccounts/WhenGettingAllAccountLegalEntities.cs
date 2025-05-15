using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.Queries.GetAllAccountLegalEntities;
using System;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.EmployerAccounts
{
    [TestFixture]
    public class WhenGettingAllAccountLegalEntities
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Account_From_Mediator(
            long accountId,
            int pageNumber,
            int pageSize,
            string sortColumn,
            bool isAscending,
            GetAllAccountLegalEntitiesQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmployerAccountsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetAllAccountLegalEntitiesQuery>(c => c.AccountId.Equals(accountId) 
                                                                && c.PageNumber.Equals(pageNumber)
                                                                && c.PageSize.Equals(pageSize)
                                                                && c.SortColumn.Equals(sortColumn)
                                                                && c.IsAscending.Equals(isAscending)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetAllAccountLegalEntities(accountId, pageNumber, pageSize, sortColumn, isAscending) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetAllAccountLegalEntitiesQueryResult;
            Assert.That(model, Is.Not.Null);
            model.LegalEntities.Should().BeEquivalentTo(mediatorResult.LegalEntities);
            model.PageInfo.Should().Be(mediatorResult.PageInfo);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            long accountId,
            int pageNumber,
            int pageSize,
            string sortColumn,
            bool isAscending,
            GetAllAccountLegalEntitiesQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmployerAccountsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetAllAccountLegalEntitiesQuery>(c => c.AccountId.Equals(accountId)
                                                                && c.PageNumber.Equals(pageNumber)
                                                                && c.PageSize.Equals(pageSize)
                                                                && c.SortColumn.Equals(sortColumn)
                                                                && c.IsAscending.Equals(isAscending)),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetAllAccountLegalEntities(accountId, pageNumber, pageSize, sortColumn, isAscending) as StatusCodeResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
