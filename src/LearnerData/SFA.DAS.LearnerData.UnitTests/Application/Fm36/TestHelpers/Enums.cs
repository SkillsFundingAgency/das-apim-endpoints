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
    ApprenticeshipWithEnglish,
    /// <summary>
    /// This scenario is for an appernticeship with 2 English and Maths courses in addition to the main apprenticeship.
    /// The maths course will conclude after the apprenticeship
    /// Learning support will span the onprogramme delivery as well as the maths course, which will continue after the 
    /// apprenticeship has completed. 
    /// </summary>
    LearningSupportComplexScenario,
    /// <summary>
    /// Models an apprenticeship with a break in learning, resulting in two onProgrammes for the same standard each with a different price
    /// </summary>
    ApprenticeshipWithBreakInLearning,
    /// <summary>
    /// Models an apprenticeship with a change of price, plus an onProgramme for a different standard which starts before the price change
    /// </summary>
    ApprenticeshipWithChangeOfStandard,
}

public enum WithdrawalDate
{
    None,
    DuringQualifyingPeriod,
    AfterQualifyingPeriod,
    BeforeNextPriceEpisodeStart,
    AfterNextPriceEpisodeStart
}