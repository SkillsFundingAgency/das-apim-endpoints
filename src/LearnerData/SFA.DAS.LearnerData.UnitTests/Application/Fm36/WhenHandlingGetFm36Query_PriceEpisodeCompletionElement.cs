using SFA.DAS.LearnerData.UnitTests.Application.Fm36.TestHelpers;

namespace SFA.DAS.LearnerData.UnitTests.Application.Fm36;

[TestFixture]
public class WhenHandlingGetFm36Query_PriceEpisodeCompletionElement
{
    private const int AcademicYearWithEarnings = 2021;

    [Test]
    public async Task AndTotalPriceIsNotGreaterThanFundingBandMaximumThenCompletionElementIsTwentyPercentOfTotalPrice()
    {
        // Arrange (simple apprenticeship: total price 15000, funding band maximum 19000)
        var testFixture = new GetFm36QueryTestFixture(TestScenario.SimpleApprenticeship);

        // Act
        await testFixture.CallSubjectUnderTest(collectionYear: AcademicYearWithEarnings);

        // Assert
        var priceEpisodes = testFixture.Result.Items.SelectMany(x => x.PriceEpisodes).ToList();
        priceEpisodes.Should().NotBeEmpty();
        priceEpisodes.Should().AllSatisfy(x => x.PriceEpisodeValues.PriceEpisodeCompletionElement.Should().Be(3000));
        priceEpisodes.Should().AllSatisfy(x => x.PriceEpisodeValues.PriceEpisodeExpectedTotalMonthlyValue.Should().Be(12000));
    }

    [Test]
    public async Task AndTotalPriceIsGreaterThanFundingBandMaximumThenCompletionElementIsTwentyPercentOfTheCappedPrice()
    {
        // Arrange (total price 15000, funding band maximum reduced to 12000, so upper limit adjustment is 3000)
        var testFixture = new GetFm36QueryTestFixture(
            TestScenario.SimpleApprenticeship,
            context => context.TestLearners.Single().FundingBandMax = 12000);

        // Act
        await testFixture.CallSubjectUnderTest(collectionYear: AcademicYearWithEarnings);

        // Assert
        var priceEpisodes = testFixture.Result.Items.SelectMany(x => x.PriceEpisodes).ToList();
        priceEpisodes.Should().NotBeEmpty();
        priceEpisodes.Should().AllSatisfy(x => x.PriceEpisodeValues.PriceEpisodeCompletionElement.Should().Be(2400));
        priceEpisodes.Should().AllSatisfy(x => x.PriceEpisodeValues.PriceEpisodeExpectedTotalMonthlyValue.Should().Be(9600));
    }
}
