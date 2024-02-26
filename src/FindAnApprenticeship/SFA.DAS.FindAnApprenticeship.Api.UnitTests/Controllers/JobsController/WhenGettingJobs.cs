using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.WorkHistory;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Models;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.JobsController
{
    [TestFixture]
    public class WhenGettingJobs
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Response_Is_Returned(
           Guid candidateId,
           Guid applicationId,
           GetJobsQueryResult queryResult,
           [Frozen] Mock<IMediator> mediator,
           [Greedy] Api.Controllers.JobsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetJobsQuery>(q =>
                        q.CandidateId == candidateId && q.ApplicationId == applicationId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var actual = await controller.GetJobs(applicationId, candidateId);

            actual.Should().BeOfType<OkObjectResult>();
            var actualObject = ((OkObjectResult)actual).Value as GetJobsApiResponse;
            actualObject.Should().NotBeNull();
            actualObject.Should().BeEquivalentTo((GetJobsApiResponse)queryResult);
        }
    }
}
