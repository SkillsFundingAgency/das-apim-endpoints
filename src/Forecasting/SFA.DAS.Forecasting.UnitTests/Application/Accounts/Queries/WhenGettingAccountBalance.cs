using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Forecasting.Application.Accounts.Queries.GetAccountBalance;
using SFA.DAS.Forecasting.InnerApi.Requests;
using SFA.DAS.Forecasting.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Forecasting.UnitTests.Application.Accounts.Queries
{
    public class WhenGettingAccountBalance
    {
        [Test, MoqAutoData]
        public async Task Then_The_EndPoint_Is_Called_And_Data_Returned(
            GetAccountBalanceQuery query,
            GetAccountBalanceResponse apiResponse,
            [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> financeApiClient,
            GetAccountBalanceQueryHandler handler)
        {
            var expectedGetUrl = new PostAccountBalanceRequest(query.AccountId);
            financeApiClient
                .Setup(x => x.PostWithResponseCode<GetAccountBalanceResponse[]>(
                    It.Is<PostAccountBalanceRequest>(c => c.PostUrl.Equals(expectedGetUrl.PostUrl)), true))
                .ReturnsAsync(new ApiResponse<GetAccountBalanceResponse[]>(new []{apiResponse}, HttpStatusCode.OK,""));

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.AccountBalance.Should().Be(apiResponse);

        }

        [Test, MoqAutoData]
        public async Task Then_If_Empty_List_Then_Null_Returned(
            GetAccountBalanceQuery query,
            [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> financeApiClient,
            GetAccountBalanceQueryHandler handler)
        {
            var expectedGetUrl = new PostAccountBalanceRequest(query.AccountId);
            financeApiClient
                .Setup(x => x.PostWithResponseCode<GetAccountBalanceResponse[]>(
                    It.Is<PostAccountBalanceRequest>(c => c.PostUrl.Equals(expectedGetUrl.PostUrl)), true))
                .ReturnsAsync(new ApiResponse<GetAccountBalanceResponse[]>(Array.Empty<GetAccountBalanceResponse>(), HttpStatusCode.OK,""));

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.AccountBalance.Should().BeNull();
        }
    }
}