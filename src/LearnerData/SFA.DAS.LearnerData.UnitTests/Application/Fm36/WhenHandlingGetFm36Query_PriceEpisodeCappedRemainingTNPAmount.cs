using SFA.DAS.LearnerData.UnitTests.Application.Fm36.TestHelpers;

namespace SFA.DAS.LearnerData.UnitTests.Application.Fm36;

[TestFixture]
public class WhenHandlingGetFm36Query_PriceEpisodeCappedRemainingTNPAmount
{
    private const int AcademicYearWithEarnings = 2021;

    [Test]
    public async Task AndTotalPriceIsNotGreaterThanFundingBandMaximumThenCappedRemainingTNPAmountIsTheTotalPrice()
    {
        // Arrange (simple apprenticeship: total price 15000, funding band maximum 19000)
        var testFixture = new GetFm36QueryTestFixture(TestScenario.SimpleApprenticeship);

        // Act
        await testFixture.CallSubjectUnderTest(collectionYear: AcademicYearWithEarnings);

        // Assert
        var priceEpisodes = testFixture.Result.Items.SelectMany(x => x.PriceEpisodes).ToList();
        priceEpisodes.Should().NotBeEmpty();
        priceEpisodes.Should().AllSatisfy(x => x.PriceEpisodeValues.PriceEpisodeCappedRemainingTNPAmount.Should().Be(15000));
    }

    [Test]
    public async Task AndTotalPriceIsGreaterThanFundingBandMaximumThenCappedRemainingTNPAmountIsTheFundingBandMaximum()
    {
        // Arrange (total price 15000, funding band maximum reduced to 12000)
        var testFixture = new GetFm36QueryTestFixture(
            TestScenario.SimpleApprenticeship,
            context => context.TestLearners.Single().FundingBandMax = 12000);

        // Act
        await testFixture.CallSubjectUnderTest(collectionYear: AcademicYearWithEarnings);

        // Assert
        var priceEpisodes = testFixture.Result.Items.SelectMany(x => x.PriceEpisodes).ToList();
        priceEpisodes.Should().NotBeEmpty();
        priceEpisodes.Should().AllSatisfy(x => x.PriceEpisodeValues.PriceEpisodeCappedRemainingTNPAmount.Should().Be(12000));
    }
}
