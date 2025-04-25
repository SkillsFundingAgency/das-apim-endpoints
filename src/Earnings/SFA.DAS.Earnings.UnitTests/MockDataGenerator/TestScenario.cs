namespace SFA.DAS.Earnings.UnitTests.MockDataGenerator
{
    public enum TestScenario
    {
        NoData,
        AllData,
        /// <summary>
        /// Models a simple one-year apprenticeship that crosses an Academic Year boundary
        /// </summary>
        SimpleApprenticeship,
        /// <summary>
        /// Models an apprenticeship with a change of price
        /// </summary>
        ApprenticeshipWithPriceChange
    }
}
