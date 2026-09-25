using SFA.DAS.Recruit.Application.Report.Query.GetReportDataById;
using SFA.DAS.Recruit.Domain.Reports;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests.Reports;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses.Reports;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SFA.DAS.Recruit.UnitTests.Application.Reports;

[TestFixture]
internal class WhenHandlingGetReportDataByIdQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Report_Data_Is_Returned(
        GetReportDataByIdQuery query,
        List<ApplicationSummaryReport> reports,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] GetReportDataByIdQueryHandler sut)
    {
        var response = new GetReportDataResponse { Reports = reports };
        GetReportDataRequest? capturedRequest = null;
        recruitApiClient
            .Setup(x => x.Get<GetReportDataResponse>(It.IsAny<GetReportDataRequest>()))
            .Callback<IGetApiRequest>(x => capturedRequest = x as GetReportDataRequest)
            .ReturnsAsync(response);

        var result = await sut.Handle(query, CancellationToken.None);

        result.Reports.Should().BeEquivalentTo(reports);
        capturedRequest.Should().NotBeNull();
        capturedRequest!.ReportId.Should().Be(query.ReportId);
    }

    [Test, MoqAutoData]
    public async Task Then_When_Response_Is_Null_Returns_Empty_List(
        GetReportDataByIdQuery query,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] GetReportDataByIdQueryHandler sut)
    {
        recruitApiClient
            .Setup(x => x.Get<GetReportDataResponse>(It.IsAny<GetReportDataRequest>()))
            .ReturnsAsync((GetReportDataResponse?)null);

        var result = await sut.Handle(query, CancellationToken.None);

        result.Reports.Should().BeEmpty();
    }
}
