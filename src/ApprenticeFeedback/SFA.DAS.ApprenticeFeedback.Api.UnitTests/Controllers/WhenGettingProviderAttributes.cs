using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Api.Controllers;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetAttributes;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Api.UnitTests.Controllers
{
    public class WhenGettingProviderAttributes
    {
        [Test, MoqAutoData]
        public async Task Then_The_Attributes_Are_Returned_From_Mediator(
            GetAttributesResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AttributesController controller)
        {
            mockMediator
                .Setup(x => x.Send(It.IsAny<GetAttributesQuery>(), CancellationToken.None))
                .ReturnsAsync(queryResult);

            var actual = await controller.GetProviderAttributes() as ObjectResult;

            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().BeEquivalentTo(queryResult.Attributes);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] AttributesController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<GetAttributesQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.GetProviderAttributes() as StatusCodeResult;

            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
