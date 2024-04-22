using System;
using SFA.DAS.ApprenticeApp.MockApis;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeApp.Api.UnitTests.Bindings
{
    [Binding]
    public static class SpecflowDictionaryCleanup
    {
        [AfterScenario]
        public static void ResetFeature(FeatureContext feature)
            => ResetAll(feature);

        [AfterScenario]
        public static void DisposeScenario(ScenarioContext scenario)
            => DisposeAll(scenario);

        [AfterFeature]
        public static void DisposeFeature(FeatureContext feature)
            => DisposeAll(feature);

        private static void ResetAll(SpecFlowContext context)
            => ApplyAll<IResettable>(context, x => x.Reset());

        private static void DisposeAll(SpecFlowContext context)
            => ApplyAll<IDisposable>(context, x => x.Dispose());

        private static void ApplyAll<T>(SpecFlowContext context, Action<T> act)
        {
            foreach (var x in context)
                if (x.Value is T t) act(t);
        }
    }
}