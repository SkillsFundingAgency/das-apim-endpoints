using SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticePortal.Api.UnitTests
{
    public class TestContext
    {
        private readonly FeatureContext _feature;

        public TestContext(FeatureContext feature) => _feature = feature;

        public ApprenticePortalOuterApi OuterApiClient { get; set; }
        public MockApi ApprenticeCommitmentsInnerApi => _feature.GetOrAdd<MockApi>();
        public MockApi ApprenticeAccountsInnerApi => _feature.GetOrAdd<MockApi>();
    }
}