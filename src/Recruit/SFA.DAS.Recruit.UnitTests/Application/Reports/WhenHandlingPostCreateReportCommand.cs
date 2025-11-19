using SFA.DAS.Recruit.Application.Report.Command.PostCreateReport;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests.Reports;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.UnitTests.Application.Reports;
[TestFixture]
internal class WhenHandlingPostCreateReportCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_Is_Returned(
        PostCreateReportCommand command,
        Recruit.Domain.Reports.Report response,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] PostCreateReportCommandHandler sut)
    {
        // arrange
        var expectedPostUrl = new PostReportRequest(new PostReportRequest.PostReportRequestData()
        {
            CreatedBy = command.CreatedBy,
            FromDate = command.FromDate,
            Id = command.Id,
            Name = command.Name,
            OwnerType = command.OwnerType,
            ToDate = command.ToDate,
            Ukprn = command.Ukprn,
            UserId = command.UserId
        });
        recruitApiClient.Setup(x => x.PostWithResponseCode<Recruit.Domain.Reports.Report>(
                It.Is<PostReportRequest>(r => r.PostUrl == expectedPostUrl.PostUrl), true))
            .ReturnsAsync(new ApiResponse<Recruit.Domain.Reports.Report>(response, HttpStatusCode.OK, string.Empty));

        // act
        await sut.Handle(command, CancellationToken.None);

        // assert
        recruitApiClient.Verify(x => x.PostWithResponseCode<Recruit.Domain.Reports.Report>(It.IsAny<PostReportRequest>(), true), Times.Once);
    }
}