using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Funding.Api.Controllers;
using SFA.DAS.Funding.Api.Models;
using SFA.DAS.Funding.Application.Queries.GetProviderAcademicYearEarnings;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Funding.Api.UnitTests.Controllers.ProviderEarnings
{
    public class WhenGettingAcademicYearEarnings
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Earnings_From_Mediator(
            long ukprn,
            GetProviderAcademicYearEarningsResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProviderEarningsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetProviderAcademicYearEarningsQuery>(c=> c.Ukprn.Equals(ukprn)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetDetail(ukprn) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetProviderAcademicYearEarningsResponse;
            Assert.That(model, Is.Not.Null);
            model.AcademicYearEarnings.Should().BeEquivalentTo(mediatorResult.AcademicYearEarnings);
        }
    }
}