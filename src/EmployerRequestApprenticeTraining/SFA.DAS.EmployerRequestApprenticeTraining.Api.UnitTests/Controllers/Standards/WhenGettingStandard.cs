using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Api.Controllers;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetStandard;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.WhenGettingStandard.Api.UnitTests.Controllers.Standards
{
    public class WhenGettingStandard
    {
        [Test, MoqAutoData]
        public async Task Then_The_Standard_Is_Returned_From_Mediator(
            string standardId,
            GetStandardResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] StandardsController controller)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<GetStandardQuery>(p => p.StandardId == standardId), CancellationToken.None))
                .ReturnsAsync(queryResult);

            var actual = await controller.Get(standardId) as ObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().BeEquivalentTo(queryResult.Standard);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            string standardId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] StandardsController controller)
        {
            mockMediator.Setup(x => x.Send(It.IsAny<GetStandardQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.Get(standardId) as StatusCodeResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
