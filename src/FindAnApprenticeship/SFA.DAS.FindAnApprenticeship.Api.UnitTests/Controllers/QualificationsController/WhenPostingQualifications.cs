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
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateQualifications;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.QualificationsController
{
    [TestFixture]
    public class WhenPostingQualifications
    {
        [Test, MoqAutoData]
        public async Task Then_The_Created_Status_Is_Returned(
            Guid applicationId,
            PostQualificationsApiRequest apiRequest,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.QualificationsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<UpdateQualificationsCommand>(c =>
                        c.CandidateId.Equals(apiRequest.CandidateId)
                        && c.ApplicationId == applicationId
                        && c.CandidateId == apiRequest.CandidateId
                        && c.IsComplete == apiRequest.IsComplete),
                    CancellationToken.None))
                .Returns(() => Task.CompletedTask);

            var actual = await controller.PostQualifications(applicationId, apiRequest);

            actual.Should().BeOfType<CreatedResult>();
        }
    }
}
