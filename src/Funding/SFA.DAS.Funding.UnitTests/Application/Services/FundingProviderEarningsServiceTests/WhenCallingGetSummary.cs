using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Funding.Application.Services;
using SFA.DAS.Funding.Configuration;
using SFA.DAS.Funding.InnerApi.Requests.ProviderEarnings;
using SFA.DAS.Funding.InnerApi.Responses;
using SFA.DAS.Funding.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Funding.UnitTests.Application.Services.FundingProviderEarningsServiceTests
{
    public class WhenCallingGetSummary
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_Returning_The_Application(
            long ukprn,
            ProviderEarningsSummaryDto apiResponse,
            [Frozen] Mock<IFundingApprenticeshipEarningsApiClient<FundingApprenticeshipEarningsConfiguration>> client,
            FundingProviderEarningsService service
        )
        {
            client.Setup(x =>
                    x.Get<ProviderEarningsSummaryDto>(
                        It.Is<GetProviderEarningsSummaryRequest>(c => c.GetUrl.Contains(ukprn.ToString()))))
                .ReturnsAsync(apiResponse);

            var actual = await service.GetSummary(ukprn);

            actual.Should().BeEquivalentTo(apiResponse);
        }
    }
}