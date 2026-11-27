#nullable enable
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.EmployerFinance.Application.Queries.GetLevyProjectionsStatusByAccountId;
using SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System;
using System.Net;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Queries.GetLevyProjectionsStatusByAccountId;

[TestFixture]
internal class WhenHandlingGetLevyProjectionsStatusByAccountIdQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Last_Successful_Import_Date_From_The_Funding_Projection_Api_Is_Returned(
        long accountId,
        GetImportJobStateResponse response,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> projectionApiClient,
        [Greedy] GetLevyProjectionsStatusByAccountIdQueryHandler sut)
    {
        GetImportJobStateRequest? capturedRequest = null;
        projectionApiClient
            .Setup(x => x.GetWithResponseCode<GetImportJobStateResponse>(It.IsAny<GetImportJobStateRequest>()))
            .Callback<IGetApiRequest>(r => capturedRequest = r as GetImportJobStateRequest)
            .ReturnsAsync(new ApiResponse<GetImportJobStateResponse>(response, HttpStatusCode.OK, null!));

        var result = await sut.Handle(new GetLevyProjectionsStatusByAccountIdQuery(accountId), CancellationToken.None);

        result.LastSuccessfulImportDate.Should().Be(response.LastSuccessfulImportDate);
        capturedRequest.Should().NotBeNull();
        capturedRequest!.GetUrl.Should().Be($"api/jobs/{accountId}/ImportLearnersCost");
    }

    [Test, MoqAutoData]
    public async Task Then_The_Minimum_Date_Is_Returned_When_The_Account_Has_No_Job_State(
        long accountId,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> projectionApiClient,
        [Greedy] GetLevyProjectionsStatusByAccountIdQueryHandler sut)
    {
        projectionApiClient
            .Setup(x => x.GetWithResponseCode<GetImportJobStateResponse>(It.IsAny<GetImportJobStateRequest>()))
            .ReturnsAsync(new ApiResponse<GetImportJobStateResponse>(null!, HttpStatusCode.NotFound, null!));

        var result = await sut.Handle(new GetLevyProjectionsStatusByAccountIdQuery(accountId), CancellationToken.None);

        result.LastSuccessfulImportDate.Should().Be(DateTime.MinValue);
    }

    [Test, MoqAutoData]
    public async Task Then_An_Exception_Is_Thrown_When_The_Funding_Projection_Api_Fails(
        long accountId,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> projectionApiClient,
        [Greedy] GetLevyProjectionsStatusByAccountIdQueryHandler sut)
    {
        projectionApiClient
            .Setup(x => x.GetWithResponseCode<GetImportJobStateResponse>(It.IsAny<GetImportJobStateRequest>()))
            .ReturnsAsync(new ApiResponse<GetImportJobStateResponse>(null!, HttpStatusCode.InternalServerError, "error"));

        var act = () => sut.Handle(new GetLevyProjectionsStatusByAccountIdQuery(accountId), CancellationToken.None);

        await act.Should().ThrowAsync<Exception>();
    }
}
