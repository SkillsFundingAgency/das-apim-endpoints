using System;
using SFA.DAS.ApprenticeCommitments.Configuration;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests
{
    public class TestContext
    {
        public ApprenticeCommitmentsApi OuterApiClient { get; set; }
        public MockApi InnerApi { get; set; }
        public MockApi LoginApi { get; set; }
        public ApprenticeLoginConfiguration LoginConfig { get; set; }
    }
}
