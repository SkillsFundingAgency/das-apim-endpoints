using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using SFA.DAS.LearnerDataJobs.Api.Controllers;
using SFA.DAS.LearnerDataJobs.Application.Commands;
using SFA.DAS.LearnerDataJobs.InnerApi;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LearnerDataJobs.Api.UnitTests.Controllers;
public class ApprenctieshipStopDateChangedLearnerDataControllerTests
{
    [Test, MoqAutoData]
    public async Task Then_Apprenticeship_Stop_date_changed_With_No_Issues(
      long providerId,
      long learnerDataId,
      GetLearnerByIdRequest request,
       LearnerDataApprenticeshipIdRequest patchRequest,
      [Frozen] Mock<IMediator> mediator,
      [Greedy] LearnersController controller)
    {
        mediator.Setup(x =>
               x.Send(
                   It.Is<AssignApprenticeshipIdCommand>(p =>
                       p.ProviderId == providerId && p.LearnerDataId == learnerDataId && p.PatchRequest == new LearnerDataApprenticeshipIdRequest() { ApprenticeshipId = null}),
                   CancellationToken.None))
           .ReturnsAsync(true);

        var result = await controller.ApprenticeshipStopDateChanged(providerId, learnerDataId, new ApprenticeshipStopRequest()
        { ApprenticeshipId = 1, IsWithDrawnAtStartOfCourse = true, LearnerDataId = 1 });

        result.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_Stops_Apprenticeship_With_Issues(
     long providerId,
     long learnerDataId,
     GetLearnerByIdRequest request,
      LearnerDataApprenticeshipIdRequest patchRequest,
     [Frozen] Mock<IMediator> mediator,
     [Greedy] LearnersController controller)
    {
        mediator.Setup(x =>
               x.Send(
                   It.Is<AssignApprenticeshipIdCommand>(p =>
                       p.ProviderId == providerId && p.LearnerDataId == learnerDataId && p.PatchRequest == new LearnerDataApprenticeshipIdRequest() { ApprenticeshipId = null }),
                   CancellationToken.None))
           .ReturnsAsync(true);

        var result = await controller.ApprenticeshipStopDateChanged(providerId, learnerDataId, new ApprenticeshipStopRequest()
        { ApprenticeshipId = 1, IsWithDrawnAtStartOfCourse = true, LearnerDataId = 1});

        result.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_Apprenticeship_Stop_date_changed_With_No_Issues_start_stop_date_not_equal(
     long providerId,
     long learnerDataId,
     GetLearnerByIdRequest request,
      LearnerDataApprenticeshipIdRequest patchRequest,
     [Frozen] Mock<IMediator> mediator,
     [Greedy] LearnersController controller)
    {
        mediator.Setup(x =>
               x.Send(
                   It.Is<AssignApprenticeshipIdCommand>(p =>
                       p.ProviderId == providerId && p.LearnerDataId == learnerDataId && p.PatchRequest == new LearnerDataApprenticeshipIdRequest() { ApprenticeshipId = null }),
                   CancellationToken.None))
           .ReturnsAsync(true);

        var result = await controller.ApprenticeshipStopDateChanged(providerId, learnerDataId, new ApprenticeshipStopRequest()
        { ApprenticeshipId = 0, IsWithDrawnAtStartOfCourse = false, LearnerDataId = 1 });

        result.Should().NotBeNull();
    }   
}
