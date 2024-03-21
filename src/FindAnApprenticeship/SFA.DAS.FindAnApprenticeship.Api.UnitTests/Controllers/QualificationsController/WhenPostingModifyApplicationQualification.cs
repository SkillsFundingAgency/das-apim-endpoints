using System;
using System.Collections.Generic;
using System.Linq;
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
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications.Qualifications;
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
        UpdateApplicationQualificationRequest.Subject subject,
        UpdateApplicationQualificationRequest request,
        GetAddQualificationQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy]Api.Controllers.QualificationsController controller)
    {
        request.Subjects = [subject];
        
        var actual = await controller.PostAddQualification(applicationId,qualificationReferenceTypeId, request) as CreatedResult;

        actual.Should().NotBeNull();
        mediator.Verify(x => x.Send(It.Is<UpdateApplicationQualificationCommand>(c=>
            c.QualificationReferenceId == qualificationReferenceTypeId
            && c.ApplicationId == applicationId
            && c.CandidateId == request.CandidateId
            && c.Subjects.FirstOrDefault().Grade == subject.Grade
            && c.Subjects.FirstOrDefault().Name == subject.Name
            && c.Subjects.FirstOrDefault().Id == subject.Id
            && c.Subjects.FirstOrDefault().AdditionalInformation == subject.AdditionalInformation
            && c.Subjects.FirstOrDefault().IsPredicted == subject.IsPredicted
            && c.Subjects.FirstOrDefault().ToYear == subject.ToYear
        ), CancellationToken.None));

    }
    [Test, MoqAutoData]
    public async Task Then_If_There_Is_An_Exception_Then_Internal_Server_Error_Returned(
        Guid applicationId,
        Guid qualificationReferenceTypeId,
        UpdateApplicationQualificationRequest request,
        [Frozen] Mock<IMediator> mediator,
        [Greedy]Api.Controllers.QualificationsController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<UpdateApplicationQualificationCommand>(), CancellationToken.None))
            .ThrowsAsync(new Exception());
        
        var actual = await controller.PostAddQualification(applicationId,qualificationReferenceTypeId, request) as StatusCodeResult;

        actual.Should().NotBeNull();
        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

    }
}