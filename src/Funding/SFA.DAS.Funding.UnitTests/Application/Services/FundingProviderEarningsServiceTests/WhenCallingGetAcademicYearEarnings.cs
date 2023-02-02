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
    public class WhenCallingGetAcademicYearEarnings
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_Returning_The_Earnings(
            long ukprn,
            AcademicYearEarningsDto apiResponse,
            [Frozen] Mock<IFundingApprenticeshipEarningsApiClient<FundingApprenticeshipEarningsConfiguration>> client,
            FundingProviderEarningsService service
        )
        {
            client.Setup(x =>
                    x.Get<AcademicYearEarningsDto>(
                        It.Is<GetProviderAcademicYearEarningsRequest>(c => c.GetUrl.Contains(ukprn.ToString()))))
                .ReturnsAsync(apiResponse);

            var actual = await service.GetAcademicYearEarnings(ukprn);

            actual.Should().BeEquivalentTo(apiResponse);
        }
    }
}