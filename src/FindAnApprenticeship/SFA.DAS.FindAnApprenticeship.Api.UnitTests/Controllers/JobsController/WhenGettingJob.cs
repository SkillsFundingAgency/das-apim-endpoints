using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetJob;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.JobsController
{
    [TestFixture]
    public class WhenGettingJob
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Response_Is_Returned(
           Guid candidateId,
           Guid jobId,
           Guid applicationId,
           GetJobQueryResult queryResult,
           [Frozen] Mock<IMediator> mediator,
           [Greedy] Api.Controllers.JobsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetJobQuery>(q =>
                        q.CandidateId == candidateId && q.ApplicationId == applicationId && q.JobId == jobId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var actual = await controller.GetJob(applicationId, jobId, candidateId);

            actual.Should().BeOfType<OkObjectResult>();
            var actualObject = ((OkObjectResult)actual).Value as GetJobApiResponse;
            actualObject.Should().NotBeNull();
            actualObject.Should().BeEquivalentTo((GetJobApiResponse)queryResult);
        }
    }
}
