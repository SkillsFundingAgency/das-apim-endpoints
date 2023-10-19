using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Funding.Application.Queries.ProviderAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Funding.UnitTests.Application.Queries.ProviderAccounts
{
    public class WhenHandlingGetRoatpV2ProviderQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Service_Is_Called_And_Flag_Returned(
            GetRoatpV2ProviderQuery query,
            GetProviderSummaryResponse serviceResponse,
            [Frozen] Mock<IRoatpV2TrainingProviderService> service,
            GetRoatpV2ProviderQueryHandler handler)
        {
            service.Setup(x => x.GetProviderSummary(query.Ukprn)).ReturnsAsync(serviceResponse);
            
            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().Be(serviceResponse.CanAccessApprenticeshipService);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Service_Returns_Null_Then_False_Returned(
            GetRoatpV2ProviderQuery query,
            GetProviderSummaryResponse serviceResponse,
            [Frozen] Mock<IRoatpV2TrainingProviderService> service,
            GetRoatpV2ProviderQueryHandler handler)
        {
            service.Setup(x => x.GetProviderSummary(query.Ukprn)).ReturnsAsync((GetProviderSummaryResponse) null);
            
            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().BeFalse();
        }
    }
}