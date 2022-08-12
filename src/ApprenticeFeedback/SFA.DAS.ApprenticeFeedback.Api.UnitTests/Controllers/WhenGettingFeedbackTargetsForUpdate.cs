using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Api.Controllers;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetFeedbackTargetsForUpdate;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Api.UnitTests.Controllers
{
    public class WhenGettingFeedbackTargetsForUpdate
    {
        [Test, MoqAutoData]
        public async Task Then_The_FeedbackTargets_Are_Returned_From_Mediator(
            int batchSize,
            GetFeedbackTargetsForUpdateResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApprenticeFeedbackTargetController controller)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<GetFeedbackTargetsForUpdateQuery>(query => query.BatchSize == batchSize), CancellationToken.None))
                .ReturnsAsync(queryResult);

            var actual = await controller.GetFeedbackTargetsForUpdate(batchSize) as ObjectResult;

            Assert.IsNotNull(actual);
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().BeEquivalentTo(queryResult.FeedbackTargetsForUpdate);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            int batchSize,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ApprenticeFeedbackTargetController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<GetFeedbackTargetsForUpdateQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.GetFeedbackTargetsForUpdate(batchSize) as StatusCodeResult;

            Assert.IsNotNull(actual);
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
