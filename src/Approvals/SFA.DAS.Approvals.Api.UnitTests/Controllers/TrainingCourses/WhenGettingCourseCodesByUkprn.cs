using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Application.TrainingCourses.Queries;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.TrainingCourses;

public class WhenGettingCourseCodesByUkprn
{
    [Test, MoqAutoData]
    public async Task Then_Gets_CourseCode_For_Ukprn(
            long ukprn,
            GetCourseCodesResult coursesResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] TrainingCoursesController controller)
    {
        mockMediator
            .Setup(m => m.Send(It.Is<GetCourseCodesQuery>(t => t.Ukprn == ukprn), default))
            .ReturnsAsync(coursesResult);

        var controllerResult = await controller.GetCourseCodes(ukprn) as ObjectResult;

        Assert.That(controllerResult, Is.Not.Null);
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetCourseCodesResponse;
        Assert.That(model, Is.Not.Null);
        model.TrainingProgrammes.Should().BeEquivalentTo(coursesResult.TrainingProgrammes);
    }

    [Test, MoqAutoData]
    public async Task Then_No_CourseCode_For_Ukprn(
            long ukprn,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] TrainingCoursesController controller)
    {
        mockMediator
            .Setup(m => m.Send(It.Is<GetCourseCodesQuery>(t => t.Ukprn == ukprn), default))
            .ReturnsAsync(new GetCourseCodesResult() { TrainingProgrammes = [] });

        var controllerResult = await controller.GetCourseCodes(ukprn) as ObjectResult;

        Assert.That(controllerResult, Is.Not.Null);
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetCourseCodesResponse;
        Assert.That(model, Is.Not.Null);
        model.TrainingProgrammes.Count().Should().Be(0);
    }
}