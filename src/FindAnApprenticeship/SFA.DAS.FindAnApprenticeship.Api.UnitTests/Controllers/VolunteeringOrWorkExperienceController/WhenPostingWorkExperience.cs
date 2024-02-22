using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateWorkExperience;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.VolunteeringOrWorkExperienceController
{
    [TestFixture]
    public class WhenPostingWorkExperience
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Response_Is_Returned(
        Guid applicationId,
        PostWorkExperienceApiRequest apiRequest,
        CreateWorkCommandResponse result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.VolunteeringOrWorkExperienceController controller)
        {
            mediator.Setup(x => x.Send(It.Is<CreateWorkCommand>(c =>
                        c.CandidateId.Equals(apiRequest.CandidateId)
                        && c.ApplicationId == applicationId
                        && c.CompanyName == apiRequest.CompanyName
                        && c.Description == apiRequest.Description
                        && c.StartDate == apiRequest.StartDate
                        && c.EndDate == apiRequest.EndDate),
                    CancellationToken.None))
                .ReturnsAsync(result);

            var actual = await controller.PostWorkExperience(applicationId, apiRequest) as CreatedResult;

            Assert.That(actual, Is.Not.Null);
            var actualModel = actual.Value as PostWorkExperienceApiResponse;
            Assert.That(actualModel, Is.Not.Null);
            actualModel.Should().BeEquivalentTo((PostWorkExperienceApiResponse)result);
        }

        [Test, MoqAutoData]
        public async Task Then_If_An_Exception_Is_Thrown_Then_Internal_Server_Error_Response_Returned(
            Guid applicationId,
            PostWorkExperienceApiRequest apiRequest,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.VolunteeringOrWorkExperienceController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<CreateWorkCommand>(),
                    CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.PostWorkExperience(applicationId, apiRequest) as StatusCodeResult;

            Assert.IsNotNull(actual);
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Test, MoqAutoData]
        public async Task Then_If_An_Null_Is_Returned_Then_Not_Found_Response_Returned(
            Guid applicationId,
            PostWorkExperienceApiRequest apiRequest,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.VolunteeringOrWorkExperienceController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<CreateWorkCommand>(),
                    CancellationToken.None))
                .ReturnsAsync(() => null);

            var actual = await controller.PostWorkExperience(applicationId, apiRequest) as StatusCodeResult;

            Assert.IsNotNull(actual);
            actual.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
