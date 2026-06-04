using System.Net;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.RecruitQa.Application.Report.Commands.PostCreateReport;
using SFA.DAS.RecruitQa.Domain.Reports;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.RecruitQa.UnitTests.Application.Commands;

public class WhenHandlingPostCreateReportCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Handled_And_Api_Called_With_Qa_OwnerType(
        PostCreateReportCommand command,
        Report reportResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        PostCreateReportCommandHandler handler)
    {
        recruitApiClient
            .Setup(x => x.PostWithResponseCode<NullResponse>(
                It.Is<PostReportRequest>(r => r.PostUrl == "api/reports"),
                false))
            .ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.Created, ""));

        await handler.Handle(command, CancellationToken.None);

        recruitApiClient.Verify(x => x.PostWithResponseCode<NullResponse>(
            It.Is<PostReportRequest>(r =>
                r.PostUrl == "api/reports" &&
                (r.Data as PostReportRequest.PostReportRequestData)!.OwnerType == "Qa" &&
                (r.Data as PostReportRequest.PostReportRequestData)!.Id == command.Id &&
                (r.Data as PostReportRequest.PostReportRequestData)!.Name == command.Name &&
                (r.Data as PostReportRequest.PostReportRequestData)!.UserId == command.UserId &&
                (r.Data as PostReportRequest.PostReportRequestData)!.FromDate == command.FromDate &&
                (r.Data as PostReportRequest.PostReportRequestData)!.ToDate == command.ToDate),
            false), Times.Once);
    }
}
