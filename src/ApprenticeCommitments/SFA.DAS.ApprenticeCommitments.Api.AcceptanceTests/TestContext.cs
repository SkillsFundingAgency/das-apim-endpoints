using SFA.DAS.ApprenticeCommitments.Configuration;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests
{
    public class TestContext
    {
        private readonly FeatureContext _feature;

        public TestContext(FeatureContext feature) => _feature = feature;

        public ApprenticeCommitmentsApi OuterApiClient { get; set; }
        public MockApi InnerApi => _feature.GetOrAdd<MockApi>();
        public MockApi ApprenticeAccountsApi => _feature.GetOrAdd<MockApi>();
        public MockApi LoginApi { get; set; }
        public MockApi CommitmentsV2InnerApi => _feature.GetOrAdd<MockApi>();
        public MockApi TrainingProviderInnerApi => _feature.GetOrAdd<MockApi>();
        public MockApi CoursesInnerApi => _feature.GetOrAdd<MockApi>();
        public ApprenticeLoginConfiguration LoginConfig { get; set; }
    }
}