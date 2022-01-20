﻿using TechTalk.SpecFlow;

namespace SFA.DAS.EmploymentCheck.Api.AcceptanceTests.Bindings
{
    [Binding]
    public class TestCleanUp
    {
        [AfterTestRun]
        public static void CleanUp()
        {
            MockServers.InnerApi.Dispose();
            OuterApi.Factory?.Dispose();
            OuterApi.Client?.Dispose();
        }
    }
}
