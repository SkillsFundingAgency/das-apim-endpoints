using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.DisabilityConfident;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.DisabilityConfidentController
{
    [TestFixture]
    public class WhenGettingDetailsDisabilityConfident
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Response_Is_Returned(
            Guid candidateId,
            Guid applicationId,
            GetDisabilityConfidentDetailsQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.DisabilityConfidentController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetDisabilityConfidentDetailsQuery>(q =>
                        q.CandidateId == candidateId && q.ApplicationId == applicationId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var actual = await controller.GetDisabilityConfidentDetails(applicationId, candidateId);

            actual.Should().BeOfType<OkObjectResult>();
            var actualObject = ((OkObjectResult)actual).Value as GetDisabilityConfidentDetailsApiResponse;
            actualObject.Should().NotBeNull();
            actualObject.Should().BeEquivalentTo((GetDisabilityConfidentDetailsApiResponse)queryResult);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Query_Response_Is_Null_Then_NotFound_Is_Returned(
            Guid candidateId,
            Guid applicationId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.DisabilityConfidentController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetDisabilityConfidentDetailsQuery>(q =>
                        q.CandidateId == candidateId && q.ApplicationId == applicationId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetDisabilityConfidentDetailsQueryResult)null);

            var actual = await controller.GetDisabilityConfidentDetails(applicationId, candidateId);

            actual.Should().BeOfType<NotFoundResult>();
        }
    }
}
