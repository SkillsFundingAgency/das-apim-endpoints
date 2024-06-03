using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateEqualityQuestionsCommand;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.EqualityQuestionsController
{
    public class WhenCallingPost
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Response_Is_Returned(
        Guid applicationId,
        UpsertAboutYouEqualityQuestionsCommandResult commandResult,
        PostEqualityQuestionsApiRequest apiRequest,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.EqualityQuestionsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<UpsertAboutYouEqualityQuestionsCommand>(c =>
                c.CandidateId.Equals(apiRequest.CandidateId)
                && c.ApplicationId == applicationId),
                CancellationToken.None))
                .ReturnsAsync(commandResult);

            var actual = await controller.Post(applicationId, apiRequest);
            var actualModel = actual.As<CreatedResult>().Value as PostEqualityQuestionsApiResponse;

            using (new AssertionScope())
            {
                actual.Should().BeOfType<CreatedResult>();
                actualModel!.Id.Should().Be(commandResult.Id);
            }
        }

        [Test, MoqAutoData]
        public async Task And_Command_Response_Is_Null_Then_NotFound_Returned(
            Guid applicationId,
            PostEqualityQuestionsApiRequest apiRequest,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.EqualityQuestionsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<UpsertAboutYouEqualityQuestionsCommand>(c =>
                c.CandidateId.Equals(apiRequest.CandidateId)
                && c.ApplicationId == applicationId),
                CancellationToken.None))
                .ReturnsAsync(() => null);

            var actual = await controller.Post(applicationId, apiRequest);

            using (new AssertionScope())
            {
                actual.Should().BeOfType<NotFoundResult>();
            }
        }

        [Test, MoqAutoData]
        public async Task And_An_Exception_Is_Thrown_Then_Internal_Server_Error_Response_Returned(
            Guid applicationId,
            PostEqualityQuestionsApiRequest apiRequest,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.EqualityQuestionsController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<UpsertAboutYouEqualityQuestionsCommand>(), CancellationToken.None)).ThrowsAsync(new Exception());

            var actual = await controller.Post(applicationId, apiRequest) as StatusCodeResult;

            using (new AssertionScope())
            {
                actual.Should().NotBeNull();
                actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
