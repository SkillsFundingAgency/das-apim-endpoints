using System.Net;
using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications.Qualifications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAddQualification;

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
        
        var actual = await controller.GetModifyQualification(applicationId,qualificationReferenceTypeId, candidateId, id) as OkObjectResult;

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
        
        var actual = await controller.GetModifyQualification(applicationId, qualificationReferenceTypeId, candidateId, id) as StatusCodeResult;

        actual.Should().NotBeNull();
        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

    }
}