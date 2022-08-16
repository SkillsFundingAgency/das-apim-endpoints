
using System.Collections.Generic;
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
using SFA.DAS.ApprenticeFeedback.Api.Controllers;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetFeedbackTransactionsToEmail;
using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;


namespace SFA.DAS.ApprenticeFeedback.Api.UnitTests.Controllers
{
    public class WhenGettingFeedbackTransaction
    {
        [Test, MoqAutoData]
        public async Task Then_GetsFeedbackTransactionsFromMediator(
                GetFeedbackTransactionsToEmailQuery request,
                GetFeedbackTransactionsToEmailResult mediatorResult,
                [Frozen] Mock<IMediator> mockMediator,
                [Greedy] FeedbackTransactionController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<int>(x => x == request.BatchSize),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            ObjectResult objectResult = await controller.GetFeedbackTransactionsToEmail(request.BatchSize) as ObjectResult;
            Assert.IsNotNull(objectResult);
            objectResult.StatusCode.Should().Be((int)HttpStatusCode.OK);

            IEnumerable<GetFeedbackTransactionsToEmailResponse> result = (IEnumerable<GetFeedbackTransactionsToEmailResponse>)objectResult.Value;
            Assert.IsNotNull(result);
        }
    }
}
