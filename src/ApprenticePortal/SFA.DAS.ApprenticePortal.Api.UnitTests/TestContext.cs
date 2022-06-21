using SFA.DAS.ApprenticePortal.MockApis;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticePortal.Api.UnitTests
{
    public class TestContext
    {
        private readonly FeatureContext _feature;

        public TestContext(FeatureContext feature) => _feature = feature;

        public ApprenticePortalOuterApi OuterApiClient { get; set; }
        public ApprenticeCommitmentsInnerApiMock ApprenticeCommitmentsInnerApi => _feature.GetOrAdd<ApprenticeCommitmentsInnerApiMock>();
        public ApprenticeAccountsInnerApiMock ApprenticeAccountsInnerApi => _feature.GetOrAdd<ApprenticeAccountsInnerApiMock>();
    }
}