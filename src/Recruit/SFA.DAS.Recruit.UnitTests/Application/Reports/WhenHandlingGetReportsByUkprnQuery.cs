using SFA.DAS.Recruit.Application.Report.Query.GetReportsByUkprn;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests.Reports;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Threading;

namespace SFA.DAS.Recruit.UnitTests.Application.Reports;
[TestFixture]
internal class WhenHandlingGetReportsByUkprnQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_Is_Returned(
        GetReportsByUkprnQuery query,
        List<Recruit.Domain.Reports.Report> response,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] GetReportsByUkprnQueryHandler sut)
    {
        // arrange
        GetReportsByUkprnRequest? capturedRequest = null;
        recruitApiClient
            .Setup(x => x.Get<List<Recruit.Domain.Reports.Report>>(It.IsAny<GetReportsByUkprnRequest>()))
            .Callback<IGetApiRequest>(x => capturedRequest = x as GetReportsByUkprnRequest)
            .ReturnsAsync(response);

        // act
        var result = await sut.Handle(query, CancellationToken.None);

        // assert
        result.Reports.Should().BeEquivalentTo(response);
        capturedRequest.Should().NotBeNull();
        capturedRequest!.Ukprn.Should().Be(query.Ukprn);
        capturedRequest.GetUrl.Should().Be($"api/reports/{query.Ukprn}/provider");
    }
}