namespace SFA.DAS.LearnerData.UnitTests.Application.Fm36.TestHelpers;

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
    ApprenticeshipWithPriceChange,
    /// <summary>
    /// Models an apprenticeship with a single English Course
    /// </summary>
    ApprenticeshipWithEnglish
}

public enum WithdrawalDate
{
    None,
    DuringQualifyingPeriod,
    AfterQualifyingPeriod,
    BeforeNextPriceEpisodeStart,
    AfterNextPriceEpisodeStart
}