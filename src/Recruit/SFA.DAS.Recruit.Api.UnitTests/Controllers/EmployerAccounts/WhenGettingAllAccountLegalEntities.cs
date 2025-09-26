using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.Queries.GetAllAccountLegalEntities;
using System;
using System.Net;
using System.Threading;
using SFA.DAS.Recruit.Api.Models;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.EmployerAccounts
{
    [TestFixture]
    public class WhenGettingAllAccountLegalEntities
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Account_From_Mediator(
            GetAllLegalEntitiesRequest request,
            GetAllAccountLegalEntitiesQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountLegalEntitiesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetAllAccountLegalEntitiesQuery>(c => c.SearchTerm == request.SearchTerm && c.AccountIds == request.AccountIds
                                                                && c.PageNumber == request.PageNumber
                                                                && c.PageSize == request.PageSize
                                                                && c.SortColumn == request.SortColumn
                                                                && c.IsAscending == request.IsAscending),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetAllAccountLegalEntities(request) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetAllAccountLegalEntitiesQueryResult;
            Assert.That(model, Is.Not.Null);
            model.LegalEntities.Should().BeEquivalentTo(mediatorResult.LegalEntities);
            model.PageInfo.Should().Be(mediatorResult.PageInfo);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            GetAllLegalEntitiesRequest request,
            GetAllAccountLegalEntitiesQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountLegalEntitiesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetAllAccountLegalEntitiesQuery>(c => c.SearchTerm == request.SearchTerm && c.AccountIds == request.AccountIds
                        && c.PageNumber == request.PageNumber
                        && c.PageSize == request.PageSize
                        && c.SortColumn == request.SortColumn
                        && c.IsAscending == request.IsAscending),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetAllAccountLegalEntities(request) as StatusCodeResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}