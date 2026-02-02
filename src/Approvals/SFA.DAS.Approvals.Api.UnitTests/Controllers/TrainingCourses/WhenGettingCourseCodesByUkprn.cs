using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Application.TrainingCourses.Queries;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.TrainingCourses;
public class WhenGettingCourseCodesByUkprn
{
    [Test, MoqAutoData]
    public async Task Then_Gets_CourseCode_For_Ukprn(
            GetCourseCodesResponse coursesResponse,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] TrainingCoursesController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetCourseCodesQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.GetStandards() as ObjectResult;

        Assert.That(controllerResult, Is.Not.Null);
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetStandardsListResponse;
        Assert.That(model, Is.Not.Null);
        model.Standards.Should().BeEquivalentTo(mediatorResult.Standards.Select(item => (GetStandardResponse)item));
    }
}
