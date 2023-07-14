using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Recruit.Application.Queries.GetProvider;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.TrainingProviderService;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetProvider
{
    public class WhenHandlingTheGetProviderQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Provider_Returned(
            GetProviderQuery query,
            TrainingProviderResponse apiResponse,
            [Frozen] Mock<ITrainingProviderService> apiService,
            GetProviderQueryHandler handler
        )
        {
            apiService.Setup(x => x.GetTrainingProviderDetails(query.Ukprn)).ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().BeEquivalentTo(apiResponse);
        }
    }
}
