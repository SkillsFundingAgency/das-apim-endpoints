using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateWhatInterestsYou;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.WhatInterestsYouController
{
    [TestFixture]
    public class WhenPostingWhatInterestsYou
    {
        [Test, MoqAutoData]
        public async Task Then_The_Created_Status_Is_Returned(
            Guid applicationId,
            PostWhatInterestsYouApiRequest apiRequest,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.WhatInterestsYouController controller)
        {
            mediator.Setup(x => x.Send(It.Is<UpdateWhatInterestsYouCommand>(c =>
                        c.CandidateId.Equals(apiRequest.CandidateId)
                        && c.ApplicationId == applicationId
                        && c.CandidateId == apiRequest.CandidateId
                        && c.AnswerText == apiRequest.AnswerText
                        && c.IsComplete == apiRequest.IsComplete),
                    CancellationToken.None))
                .ReturnsAsync(Unit.Value);

            var actual = await controller.PostWhatInterestsYou(applicationId, apiRequest);

            actual.Should().BeOfType<CreatedResult>();
        }
    }
}
