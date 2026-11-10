using System.Threading;
using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.RecruitJobs.Api.Controllers;
using SFA.DAS.RecruitJobs.Api.Core.BackgroundWork;
using SFA.DAS.RecruitJobs.Handlers;

namespace SFA.DAS.RecruitJobs.Api.UnitTests.Controllers.ReportsControllerTests;

public class WhenCallingPostGenerateReport
{
    [Test, MoqAutoData]
    public void Then_Generation_Is_Queued_And_Accepted_Is_Returned(
        Guid id,
        Mock<IBackgroundWorkQueue> backgroundWorkQueue,
        [Greedy] ReportsController controller)
    {
        var result = controller.PostGenerateReport(backgroundWorkQueue.Object, id);

        result.Should().BeOfType<Accepted>();
        backgroundWorkQueue.Verify(x => x.Enqueue($"GenerateReport:{id}", It.IsAny<BackgroundWorkItem>()), Times.Once());
    }

    [Test, MoqAutoData]
    public async Task Then_Queued_Work_Runs_The_Generate_Report_Handler(
        Guid id,
        Mock<IBackgroundWorkQueue> backgroundWorkQueue,
        Mock<IServiceProvider> serviceProvider,
        Mock<IGenerateReportHandler> generateReportHandler,
        [Greedy] ReportsController controller)
    {
        BackgroundWorkItem? workItem = null;
        backgroundWorkQueue
            .Setup(x => x.Enqueue(It.IsAny<string>(), It.IsAny<BackgroundWorkItem>()))
            .Callback<string, BackgroundWorkItem>((_, item) => workItem = item);
        serviceProvider
            .Setup(x => x.GetService(typeof(IGenerateReportHandler)))
            .Returns(generateReportHandler.Object);
        using var cts = new CancellationTokenSource();

        controller.PostGenerateReport(backgroundWorkQueue.Object, id);
        await workItem!(serviceProvider.Object, cts.Token);

        generateReportHandler.Verify(x => x.HandleAsync(id, cts.Token), Times.Once());
    }
}
