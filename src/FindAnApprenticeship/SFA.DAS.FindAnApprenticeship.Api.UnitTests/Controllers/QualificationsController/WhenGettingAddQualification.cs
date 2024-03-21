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
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAddQualification;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.QualificationsController;

public class WhenGettingAddQualification
{
    [Test, MoqAutoData]
    public async Task Then_The_Request_Is_Handled_And_Data_Returned(
        Guid applicationId,
        Guid qualificationReferenceTypeId,
        Guid candidateId,
        Guid? id,
        GetAddQualificationQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy]Api.Controllers.QualificationsController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetAddQualificationQuery>(c=>
                c.QualificationReferenceTypeId == qualificationReferenceTypeId
                && c.ApplicationId == applicationId
                && c.CandidateId == candidateId
                && c.Id == id
                ), CancellationToken.None))
            .ReturnsAsync(queryResult);
        
        var actual = await controller.GetAddQualification(applicationId,qualificationReferenceTypeId, candidateId, id) as OkObjectResult;

        actual.Should().NotBeNull();
        var actualValue = actual!.Value as GetQualificationReferenceTypeApiResponse;
        actualValue!.QualificationType.Should().BeEquivalentTo(queryResult.QualificationType);

    }
    [Test, MoqAutoData]
    public async Task Then_If_There_Is_An_Exception_Then_Internal_Server_Error_Returned(
        Guid applicationId,
        Guid qualificationReferenceTypeId,
        Guid candidateId,
        Guid? id,
        GetAddQualificationQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy]Api.Controllers.QualificationsController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<GetAddQualificationQuery>(), CancellationToken.None))
            .ThrowsAsync(new Exception());
        
        var actual = await controller.GetAddQualification(applicationId, qualificationReferenceTypeId, candidateId, id) as StatusCodeResult;

        actual.Should().NotBeNull();
        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

    }
}