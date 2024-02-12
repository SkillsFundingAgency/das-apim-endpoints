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
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateJob;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.JobsController
{
    [TestFixture]
    public class WhenPostingUpdateJob
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Response_Is_Returned(
        Guid applicationId,
        Guid jobId,
        PostUpdateJobApiRequest apiRequest,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.JobsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<UpdateJobCommand>(c =>
                    c.JobId == jobId
                    && c.CandidateId.Equals(apiRequest.CandidateId)
                    && c.ApplicationId == applicationId
                    && c.Employer == apiRequest.EmployerName
                    && c.Description == apiRequest.JobDescription
                    && c.JobTitle == apiRequest.JobTitle
                    && c.StartDate == apiRequest.StartDate
                    && c.EndDate == apiRequest.EndDate),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            var actual = await controller.PostUpdateJob(applicationId, jobId, apiRequest);

            actual.Should().BeOfType<OkResult>();
        }
    }
}
