using KellermanSoftware.CompareNetObjects;

namespace SFA.DAS.Forecasting.Api.UnitTests.Helpers
{
    public static class CompareHelper
    {
        public static bool AreEqualIgnoringTypes(object object1, object object2)
        {
            var compareLogic = new CompareLogic(new ComparisonConfig { IgnoreObjectTypes = true });
            var result = compareLogic.Compare(object1, object2);
            return result.AreEqual;
        }
    }
}
