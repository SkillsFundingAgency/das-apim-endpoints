using SFA.DAS.LearnerData.UnitTests.Application.Fm36.TestHelpers;

namespace SFA.DAS.LearnerData.UnitTests.Application.Fm36;

[TestFixture]
public class WhenHandlingGetFm36Query_PriceEpisodeAimSeqNumber
{
    private const int AcademicYearWithEarnings = 2021;

    [Test]
    public async Task AndThereIsAPriceChangeThenAllPriceEpisodesHaveTheOnProgrammeAimSeqNumber()
    {
        // Arrange (single onProgramme with two costs, so two price episodes)
        var testFixture = new GetFm36QueryTestFixture(
            TestScenario.ApprenticeshipWithPriceChange,
            context => context.TestLearners.Single().UpdateLearnerRequest.Delivery.OnProgramme.Single().AimSequenceNumber = 3);

        // Act
        await testFixture.CallSubjectUnderTest(collectionYear: AcademicYearWithEarnings);

        // Assert
        var priceEpisodes = testFixture.Result.Items.SelectMany(x => x.PriceEpisodes).ToList();
        priceEpisodes.Should().HaveCount(2);
        priceEpisodes.Should().AllSatisfy(x => x.PriceEpisodeValues.PriceEpisodeAimSeqNumber.Should().Be(3));
    }

    [TestCase(1920)]
    [TestCase(2021)]
    public async Task AndThePriceEpisodeIsSplitByAcademicYearThenTheAimSeqNumberIsRetained(int collectionYear)
    {
        // Arrange (simple apprenticeship runs from 2020-01-01 to 2021-01-01, so crosses the academic year boundary)
        var testFixture = new GetFm36QueryTestFixture(
            TestScenario.SimpleApprenticeship,
            context => context.TestLearners.Single().UpdateLearnerRequest.Delivery.OnProgramme.Single().AimSequenceNumber = 5);

        // Act
        await testFixture.CallSubjectUnderTest(collectionYear: collectionYear);

        // Assert
        var priceEpisodes = testFixture.Result.Items.SelectMany(x => x.PriceEpisodes).ToList();
        priceEpisodes.Should().NotBeEmpty();
        priceEpisodes.Should().AllSatisfy(x => x.PriceEpisodeValues.PriceEpisodeAimSeqNumber.Should().Be(5));
    }

    [Test]
    public async Task AndThereIsABreakInLearningThenEachPriceEpisodeHasTheAimSeqNumberOfItsOnProgramme()
    {
        // Arrange (two onProgrammes for the same standard either side of a break in learning, each with its own price)
        var testFixture = new GetFm36QueryTestFixture(TestScenario.ApprenticeshipWithBreakInLearning);

        // Act
        await testFixture.CallSubjectUnderTest(collectionYear: AcademicYearWithEarnings);

        // Assert
        var priceEpisodes = testFixture.Result.Items.SelectMany(x => x.PriceEpisodes).ToList();
        priceEpisodes
            .OrderBy(x => x.PriceEpisodeValues.EpisodeStartDate)
            .Select(x => x.PriceEpisodeValues.PriceEpisodeAimSeqNumber)
            .Should().Equal(1, 2);
    }

    [Test]
    public async Task AndThereIsAnOnProgrammeForAnotherStandardThenItIsNotUsedForThePriceEpisodes()
    {
        // Arrange (a price change on the apprenticeship's standard starts after an onProgramme for another standard)
        var testFixture = new GetFm36QueryTestFixture(TestScenario.ApprenticeshipWithChangeOfStandard);

        // Act
        await testFixture.CallSubjectUnderTest(collectionYear: AcademicYearWithEarnings);

        // Assert
        var priceEpisodes = testFixture.Result.Items.SelectMany(x => x.PriceEpisodes).ToList();
        priceEpisodes.Should().HaveCount(2);
        priceEpisodes.Should().AllSatisfy(x => x.PriceEpisodeValues.PriceEpisodeAimSeqNumber.Should().Be(1));
    }
}
