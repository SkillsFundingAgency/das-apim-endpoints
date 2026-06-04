using System.Net;
using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications.Qualifications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetQualificationTypes;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.QualificationsController;

public class WhenGettingQualificationReferenceTypes
{
    [Test, MoqAutoData]
    public async Task Then_The_Request_Is_Handled_And_Data_Returned(
        Guid applicationId,
        Guid candidateId,
        GetQualificationTypesQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy]Api.Controllers.QualificationsController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<GetQualificationTypesQuery>(), CancellationToken.None))
            .ReturnsAsync(queryResult);
        
        var actual = await controller.GetAddSelectType(applicationId, candidateId) as OkObjectResult;

        actual.Should().NotBeNull();
        var actualValue = actual!.Value as GetQualificationReferenceTypesApiResponse;
        actualValue.QualificationTypes.Should().BeEquivalentTo(queryResult.QualificationTypes);

    }
    [Test, MoqAutoData]
    public async Task Then_If_There_Is_An_Exception_Then_Internal_Server_Error_Returned(
        Guid applicationId,
        Guid candidateId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy]Api.Controllers.QualificationsController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<GetQualificationTypesQuery>(), CancellationToken.None))
            .ThrowsAsync(new Exception());
        
        var actual = await controller.GetAddSelectType(applicationId, candidateId) as StatusCodeResult;

        actual.Should().NotBeNull();
        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

    }
}