namespace SFA.DAS.Earnings.UnitTests.MockDataGenerator
{
    public enum TestScenario
    {
        AllData,
        /// <summary>
        /// Models a simple one-year apprenticeship that crosses an Academic Year boundary
        /// </summary>
        SimpleApprenticeship,
        /// <summary>
        /// Models an apprenticeship with a change of price
        /// </summary>
        ApprenticeshipWithPriceChange,
        /// <summary>
        /// Models an apprenticeship withdrawn back to the beginning, without any earnings
        /// </summary>
        WithdrawnApprenticeship
    }
}
