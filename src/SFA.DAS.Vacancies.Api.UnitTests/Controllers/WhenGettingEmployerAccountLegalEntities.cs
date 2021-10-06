using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Vacancies.Api.Controllers;
using SFA.DAS.Vacancies.Api.Models;
using SFA.DAS.Vacancies.Application.EmployerAccounts.Queries.GetLegalEntitiesForEmployer;
using SFA.DAS.Vacancies.Application.Providers.Queries.GetProviderAccountLegalEntities;

namespace SFA.DAS.Vacancies.Api.UnitTests.Controllers
{
    public class WhenGettingEmployerAccountLegalEntities
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Account_Legal_Entities_From_Mediator(
            string encodedAccountId,
            GetLegalEntitiesForEmployerResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmployerAccountLegalEntitiesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetLegalEntitiesForEmployerQuery>(c=>c.EncodedAccountId.Equals(encodedAccountId)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetList(encodedAccountId) as ObjectResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetEmployerAccountLegalEntitiesListResponse;
            model!.Should().BeEquivalentTo((GetEmployerAccountLegalEntitiesListResponse)mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            string encodedAccountId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmployerAccountLegalEntitiesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetLegalEntitiesForEmployerQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetList(encodedAccountId) as StatusCodeResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}