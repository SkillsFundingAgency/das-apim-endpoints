using SFA.DAS.Recruit.Application.Report.Query.GetReportById;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests.Reports;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;

namespace SFA.DAS.Recruit.UnitTests.Application.Reports;
[TestFixture]
internal class WhenHandlingGetReportByIdQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_Is_Returned(
        GetReportByIdQuery query,
        Recruit.Domain.Reports.Report response,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] GetReportByIdQueryHandler sut)
    {
        // arrange
        GetReportByIdRequest? capturedRequest = null;
        recruitApiClient
            .Setup(x => x.Get<Recruit.Domain.Reports.Report>(It.IsAny<GetReportByIdRequest>()))
            .Callback<IGetApiRequest>(x => capturedRequest = x as GetReportByIdRequest)
            .ReturnsAsync(response);

        // act
        var result = await sut.Handle(query, CancellationToken.None);

        // assert
        result.Report.Should().BeEquivalentTo(response);
        capturedRequest.Should().NotBeNull();
        capturedRequest!.ReportId.Should().Be(query.ReportId);
    }
}