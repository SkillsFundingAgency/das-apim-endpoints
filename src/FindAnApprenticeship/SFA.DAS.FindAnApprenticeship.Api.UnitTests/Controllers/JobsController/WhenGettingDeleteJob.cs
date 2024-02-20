using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.WorkHistory.DeleteJob;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.JobsController
{
        [TestFixture]
        public class WhenGettingDeleteJob
        {
            [Test, MoqAutoData]
            public async Task Then_The_Query_Response_Is_Returned(
               Guid candidateId,
               Guid jobId,
               Guid applicationId,
               GetDeleteJobQueryResult queryResult,
               [Frozen] Mock<IMediator> mediator,
               [Greedy] Api.Controllers.JobsController controller)
            {
                mediator.Setup(x => x.Send(It.Is<GetDeleteJobQuery>(q =>
                            q.CandidateId == candidateId && q.ApplicationId == applicationId && q.JobId == jobId),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(queryResult);

                var actual = await controller.GetDeleteJob(applicationId,candidateId, jobId);

                actual.Should().BeOfType<OkObjectResult>();
                var actualObject = ((OkObjectResult)actual).Value as GetDeleteJobApiResponse;
                actualObject.Should().NotBeNull();
                actualObject.Should().BeEquivalentTo((GetDeleteJobApiResponse)queryResult);
            }
        }
    }

