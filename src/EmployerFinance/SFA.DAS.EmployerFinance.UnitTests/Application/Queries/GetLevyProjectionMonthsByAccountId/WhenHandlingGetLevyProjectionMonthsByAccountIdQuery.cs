#nullable enable
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.EmployerFinance.Application.Queries.GetLevyProjectionMonthsByAccountId;
using SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Queries.GetLevyProjectionMonthsByAccountId;

[TestFixture]
internal class WhenHandlingGetLevyProjectionMonthsByAccountIdQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Months_From_The_Funding_Projection_Api_Are_Returned(
        long accountId,
        GetProjectionMonthsResponse response,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> projectionApiClient,
        [Greedy] GetLevyProjectionMonthsByAccountIdQueryHandler sut)
    {
        GetProjectionMonthsRequest? capturedRequest = null;
        projectionApiClient
            .Setup(x => x.Get<GetProjectionMonthsResponse>(It.IsAny<GetProjectionMonthsRequest>()))
            .Callback<IGetApiRequest>(r => capturedRequest = r as GetProjectionMonthsRequest)
            .ReturnsAsync(response);

        var result = await sut.Handle(new GetLevyProjectionMonthsByAccountIdQuery(accountId), CancellationToken.None);

        result.Months.Should().Be(response.Months);
        capturedRequest.Should().NotBeNull();
        capturedRequest!.GetUrl.Should().Be($"api/employer/{accountId}/funding-projection/months");
    }

    [Test, MoqAutoData]
    public async Task Then_An_Exception_Is_Thrown_When_The_Months_Cannot_Be_Retrieved(
        long accountId,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> projectionApiClient,
        [Greedy] GetLevyProjectionMonthsByAccountIdQueryHandler sut)
    {
        projectionApiClient
            .Setup(x => x.Get<GetProjectionMonthsResponse>(It.IsAny<GetProjectionMonthsRequest>()))
            .ReturnsAsync((GetProjectionMonthsResponse)null!);

        var act = () => sut.Handle(new GetLevyProjectionMonthsByAccountIdQuery(accountId), CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
