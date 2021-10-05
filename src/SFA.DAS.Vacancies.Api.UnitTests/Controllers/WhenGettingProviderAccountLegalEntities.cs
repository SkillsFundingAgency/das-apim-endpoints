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
using SFA.DAS.Vacancies.Application.Providers.Queries.GetProviderAccountLegalEntities;

namespace SFA.DAS.Vacancies.Api.UnitTests.Controllers
{
    public class WhenGettingProviderAccountLegalEntities
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Account_Legal_Entities_From_Mediator(
            int ukprn,
            GetProviderAccountLegalEntitiesQueryResponse mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProviderAccountLegalEntitiesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetProviderAccountLegalEntitiesQuery>(c=>c.Ukprn.Equals(ukprn)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetList(ukprn) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetProviderAccountLegalEntitiesListResponse;
            Assert.IsNotNull(model);
            model.ProviderAccountLegalEntities.Should().BeEquivalentTo(mediatorResult.ProviderAccountLegalEntities.Select(c => (GetProviderAccountLegalEntitiesListItem)c));
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            int ukprn,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProviderAccountLegalEntitiesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetProviderAccountLegalEntitiesQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetList(ukprn) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}