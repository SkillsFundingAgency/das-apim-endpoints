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
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DisabilityConfident;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.DisabilityConfidentController
{
    [TestFixture]
    public class WhenPostingDisabilityConfident
    {
        [Test, MoqAutoData]
        public async Task Then_The_Created_Status_Is_Returned(
            Guid applicationId,
            PostDisabilityConfidentApiRequest apiRequest,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.DisabilityConfidentController controller)
        {
            mediator.Setup(x => x.Send(It.Is<UpdateDisabilityConfidentCommand>(c =>
                        c.CandidateId.Equals(apiRequest.CandidateId)
                        && c.ApplicationId == applicationId
                        && c.CandidateId == apiRequest.CandidateId
                        && c.ApplyUnderDisabilityConfidentScheme == apiRequest.ApplyUnderDisabilityConfidentScheme),
                    CancellationToken.None))
                .Returns(() => Task.CompletedTask);

            var actual = await controller.PostDisabilityConfident(applicationId, apiRequest);

            actual.Should().BeOfType<OkResult>();
        }
    }
}
