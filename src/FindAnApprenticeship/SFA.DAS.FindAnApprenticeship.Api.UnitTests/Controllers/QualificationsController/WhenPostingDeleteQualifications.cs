using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications.Qualifications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteQualifications;
using SFA.DAS.Testing.AutoFixture;


namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.QualificationsController
{
    [TestFixture]
    public class WhenPostingDeleteQualifications
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Response_Is_Returned(
            Guid qualificationReferenceId,
            Guid applicationId,
            PostDeleteQualificationsApiRequest request,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.QualificationsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<DeleteQualificationsCommand>(c =>
                    c.CandidateId == request.CandidateId
                    && c.ApplicationId == applicationId
                    && c.QualificationReferenceId == qualificationReferenceId),
                CancellationToken.None)).Returns(Task.CompletedTask);

            var actual = await controller.PostDeleteQualifications(applicationId, qualificationReferenceId, request);

            actual.Should().BeOfType<OkResult>();
        }
    }
}
