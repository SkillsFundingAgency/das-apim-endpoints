using System;
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

namespace SFA.DAS.Vacancies.Manage.Api.UnitTests.Controllers
{
    public class WhenGettingQualifications
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Qualifications_From_Mediator(
            int ukprn,
            GetQualificationsQueryResponse mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] QualificationsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetQualificationsQuery>(c=>c.Ukprn.Equals(ukprn)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetList(ukprn) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetQualificationsQueryResponse;
            Assert.IsNotNull(model);
            model.Qualifications.Should().BeEquivalentTo(mediatorResult.Qualifications.Select(c => (GetQualificationsListItem)c));
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            int ukprn,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] QualificationsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetQualificationsQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetList(ukprn) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}