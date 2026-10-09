using SFA.DAS.EmployerFinance.Application.Queries.GetLevyProjectionsByAccountId;
using SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;
using SFA.DAS.EmployerFinance.Models.Enums;
using SFA.DAS.EmployerFinance.Models.Projections;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.EmployerFinance.InnerApi.Responses.Finance;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Queries.GetLevyProjectionsByAccountId;

[TestFixture]
internal class WhenHandlingGetLevyProjectionsByAccountIdQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Projection_Is_Returned(
        long accountId,
        DateTime lastSubmissionDate,
        GetLevySummaryByAccountIdResponse levySummaryByAccountIdResponse,
        EstimatesTimeline estimatesTimeline,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> financeApiClient,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> projectionApiClient,
        [Greedy] GetLevyProjectionsByAccountIdQueryHandler sut)
    {
        // arrange
        var now = DateTime.UtcNow;
        var request = new GetLevyProjectionsByAccountIdQuery(accountId);

        GetLevyLastSubmissionDateRequest? capturedLastSubmissionDateRequest = null;
        financeApiClient
            .Setup(x => x.Get<GetLevyLastSubmissionDateResponse>(It.IsAny<GetLevyLastSubmissionDateRequest>()))
            .Callback<IGetApiRequest>(r => capturedLastSubmissionDateRequest = r as GetLevyLastSubmissionDateRequest)
            .ReturnsAsync(new GetLevyLastSubmissionDateResponse { LastSubmissionDate = lastSubmissionDate });

        GetLevySummaryByAccountIdRequest? capturedLevySummaryByAccountIdRequest = null;
        financeApiClient
            .Setup(x => x.Get<GetLevySummaryByAccountIdResponse>(It.IsAny<GetLevySummaryByAccountIdRequest>()))
            .Callback<IGetApiRequest>(r => capturedLevySummaryByAccountIdRequest = r as GetLevySummaryByAccountIdRequest)
            .ReturnsAsync(levySummaryByAccountIdResponse);
        
        GetAccountTransactionSummaryByDateRequest? capturedAccountTransactionSummaryByDateRequest = null;
        financeApiClient
            .Setup(x => x.Get<List<TransactionLine>>(It.IsAny<GetAccountTransactionSummaryByDateRequest>()))
            .Callback<IGetApiRequest>(r => capturedAccountTransactionSummaryByDateRequest = r as GetAccountTransactionSummaryByDateRequest)
            .ReturnsAsync([
                new TransactionLine { TransactionDate = now, Amount = 500m, TransactionType = TransactionItemType.Declaration },
                new TransactionLine { TransactionDate = now, Amount = 500m, TransactionType = TransactionItemType.ShortExpiredFund },
                new TransactionLine { TransactionDate = now, Amount = 100m, TransactionType = TransactionItemType.ExpiredFund },
            ]);
        
        PostEmployerFundingProjectionRequest? capturedEmployerFundingProjectionRequest = null;
        projectionApiClient
            .Setup(x => x.PostWithResponseCode<EstimatesTimeline>(It.IsAny<PostEmployerFundingProjectionRequest>(), true))
            .Callback<IPostApiRequest, bool>((r, _) => capturedEmployerFundingProjectionRequest = r as PostEmployerFundingProjectionRequest)
            .ReturnsAsync(new ApiResponse<EstimatesTimeline>(estimatesTimeline, HttpStatusCode.OK, null!));

        List<LevyInMonthSummary> expectedHistoricLevyIn = [new(new DateOnly(now.Year, now.Month, 1), 500m)];
        
        // act
        var result = await sut.Handle(request, CancellationToken.None);

        // assert
        capturedLastSubmissionDateRequest.Should().NotBeNull();
        capturedLastSubmissionDateRequest.AccountId.Should().Be(request.AccountId);
        
        capturedLevySummaryByAccountIdRequest.Should().NotBeNull();
        capturedLevySummaryByAccountIdRequest.AccountId.Should().Be(request.AccountId);
        
        capturedAccountTransactionSummaryByDateRequest.Should().NotBeNull();
        capturedAccountTransactionSummaryByDateRequest.AccountId.Should().Be(request.AccountId);
        
        capturedEmployerFundingProjectionRequest.Should().NotBeNull();
        capturedEmployerFundingProjectionRequest.AccountId.Should().Be(request.AccountId);
        capturedEmployerFundingProjectionRequest.PostData.Months.Should().Be(6);
        capturedEmployerFundingProjectionRequest.PostData.HistoricLevyIn.Should().BeEquivalentTo(expectedHistoricLevyIn);

        result.AccountId.Should().Be(request.AccountId);
        result.Summary.Should().BeEquivalentTo(levySummaryByAccountIdResponse);
        result.Projections.Should().BeEquivalentTo(estimatesTimeline.Projections);
        result.LatestLevyDeclarationInDate.Should().Be(lastSubmissionDate);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Handler_Sums_The_Transactions_Before_Projection(
        long accountId,
        DateTime lastSubmissionDate,
        GetLevySummaryByAccountIdResponse levySummaryByAccountIdResponse,
        EstimatesTimeline estimatesTimeline,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> financeApiClient,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> projectionApiClient,
        [Greedy] GetLevyProjectionsByAccountIdQueryHandler sut)
    {
        // arrange
        var now = DateTime.UtcNow;
        var request = new GetLevyProjectionsByAccountIdQuery(accountId);

        financeApiClient
            .Setup(x => x.Get<GetLevyLastSubmissionDateResponse>(It.IsAny<GetLevyLastSubmissionDateRequest>()))
            .ReturnsAsync(new GetLevyLastSubmissionDateResponse { LastSubmissionDate = lastSubmissionDate });

        financeApiClient
            .Setup(x => x.Get<GetLevySummaryByAccountIdResponse>(It.IsAny<GetLevySummaryByAccountIdRequest>()))
            .ReturnsAsync(levySummaryByAccountIdResponse);
        
        financeApiClient
            .Setup(x => x.Get<List<TransactionLine>>(It.IsAny<GetAccountTransactionSummaryByDateRequest>()))
            .ReturnsAsync([
                new TransactionLine { TransactionDate = now, Amount = 500m, TransactionType = TransactionItemType.Declaration },
                new TransactionLine { TransactionDate = now, Amount = 500m, TransactionType = TransactionItemType.Declaration },
            ]);
        
        PostEmployerFundingProjectionRequest? capturedEmployerFundingProjectionRequest = null;
        projectionApiClient
            .Setup(x => x.PostWithResponseCode<EstimatesTimeline>(It.IsAny<PostEmployerFundingProjectionRequest>(), true))
            .Callback<IPostApiRequest, bool>((r, _) => capturedEmployerFundingProjectionRequest = r as PostEmployerFundingProjectionRequest)
            .ReturnsAsync(new ApiResponse<EstimatesTimeline>(estimatesTimeline, HttpStatusCode.OK, null!));

        List<LevyInMonthSummary> expectedHistoricLevyIn = [new(new DateOnly(now.Year, now.Month, 1), 1000m)];
        
        // act
        await sut.Handle(request, CancellationToken.None);

        // assert
        capturedEmployerFundingProjectionRequest.Should().NotBeNull();
        capturedEmployerFundingProjectionRequest.PostData.HistoricLevyIn.Should().BeEquivalentTo(expectedHistoricLevyIn);
    }
}