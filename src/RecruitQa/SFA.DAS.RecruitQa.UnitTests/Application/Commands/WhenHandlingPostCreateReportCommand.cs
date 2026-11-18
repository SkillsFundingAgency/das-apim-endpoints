using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.RecruitQa.Application.Report.Commands.PostCreateReport;
using System.Net;
using IRecruitApiClient = SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>;

namespace SFA.DAS.RecruitQa.UnitTests.Application.Commands;

public class WhenHandlingPostCreateReportCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Handled_And_Api_Called_With_Qa_OwnerType(
        PostCreateReportCommand command,
        [Frozen] Mock<IRecruitApiClient> recruitApiClient,
        PostCreateReportCommandHandler handler)
    {
        recruitApiClient
            .Setup(x => x.PostWithResponseCode<NullResponse>(It.IsAny<PostReportsApiRequest>()))
            .ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.Created, ""));

        await handler.Handle(command, CancellationToken.None);

        recruitApiClient.Verify(x => x.PostWithResponseCode<NullResponse>(It.IsAny<PostReportsApiRequest>()), Times.Once());
    }
}