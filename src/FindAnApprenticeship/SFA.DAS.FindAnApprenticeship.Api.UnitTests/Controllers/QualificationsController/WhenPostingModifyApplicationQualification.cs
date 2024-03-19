using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateApplicationQualification;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAddQualification;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.QualificationsController;

public class WhenPostingModifyApplicationQualification
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Handled_And_Created_Response_Returned(
        Guid applicationId,
        Guid qualificationReferenceTypeId,
        Guid candidateId,
        UpdateApplicationQualificationRequest request,
        GetAddQualificationQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy]Api.Controllers.QualificationsController controller)
    {
        
        var actual = await controller.PostAddQualification(applicationId,qualificationReferenceTypeId, candidateId, request) as CreatedResult;

        actual.Should().NotBeNull();
        mediator.Verify(x => x.Send(It.Is<UpdateApplicationQualificationCommand>(c=>
            c.QualificationReferenceId == qualificationReferenceTypeId
            && c.ApplicationId == applicationId
            && c.CandidateId == candidateId
            && c.Grade == request.Grade
            && c.Subject == request.Subject
            && c.Id == request.Id
            && c.AdditionalInformation == request.AdditionalInformation
            && c.IsPredicted == request.IsPredicted
            && c.ToYear == request.ToYear
        ), CancellationToken.None));

    }
    [Test, MoqAutoData]
    public async Task Then_If_There_Is_An_Exception_Then_Internal_Server_Error_Returned(
        Guid applicationId,
        Guid qualificationReferenceTypeId,
        Guid candidateId,
        UpdateApplicationQualificationRequest request,
        [Frozen] Mock<IMediator> mediator,
        [Greedy]Api.Controllers.QualificationsController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<UpdateApplicationQualificationCommand>(), CancellationToken.None))
            .ThrowsAsync(new Exception());
        
        var actual = await controller.PostAddQualification(applicationId,qualificationReferenceTypeId, candidateId, request) as StatusCodeResult;

        actual.Should().NotBeNull();
        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

    }
}