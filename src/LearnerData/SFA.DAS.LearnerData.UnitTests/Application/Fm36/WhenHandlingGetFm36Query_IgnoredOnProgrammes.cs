using SFA.DAS.LearnerData.UnitTests.Application.Fm36.TestHelpers;

namespace SFA.DAS.LearnerData.UnitTests.Application.Fm36;

/// <summary>
/// The SLD cache holds every on programme aim for a learner, but only the aims for the same standard and agreement as the
/// earliest aim (the first apprenticeship) are sent to Learning (and so earned on). Any other aims are ignored and must not be returned in the FM36 block.
/// </summary>
[TestFixture]
public class WhenHandlingGetFm36Query_IgnoredOnProgrammes
{
    private const int AcademicYearWithEarnings = 2021;

    [Test]
    public async Task AndThereIsAnOnProgrammeForAnotherStandardThenNoLearningDeliveryIsReturnedForIt()
    {
        // Arrange (aim 1 is the first apprenticeship, aim 2 is for another standard so is ignored)
        var testFixture = new GetFm36QueryTestFixture(TestScenario.ApprenticeshipWithChangeOfStandard);

        // Act
        await testFixture.CallSubjectUnderTest(collectionYear: AcademicYearWithEarnings);

        // Assert
        var learner = testFixture.Result.Items.Single();
        learner.LearningDeliveries.Select(x => x.AimSeqNumber).Should().Equal(1);
        learner.PriceEpisodes.Should().AllSatisfy(x => x.PriceEpisodeValues.PriceEpisodeAimSeqNumber.Should().Be(1));
    }

    [Test]
    public async Task AndThereIsAnOnProgrammeForAnotherAgreementThenNoLearningDeliveryIsReturnedForIt()
    {
        // Arrange (two onProgrammes for the same standard either side of a break in learning, but the second is for another agreement so is ignored)
        var testFixture = new GetFm36QueryTestFixture(
            TestScenario.ApprenticeshipWithBreakInLearning,
            context => context.TestLearners.Single().UpdateLearnerRequest.Delivery.OnProgramme
                .Single(x => x.AimSequenceNumber == 2).AgreementId = "AnotherAgreement");

        // Act
        await testFixture.CallSubjectUnderTest(collectionYear: AcademicYearWithEarnings);

        // Assert
        var learner = testFixture.Result.Items.Single();
        learner.LearningDeliveries.Select(x => x.AimSeqNumber).Should().Equal(1);
        learner.PriceEpisodes.Should().AllSatisfy(x => x.PriceEpisodeValues.PriceEpisodeAimSeqNumber.Should().Be(1));
    }

    [Test]
    public async Task AndAllOnProgrammesAreForTheFirstApprenticeshipThenALearningDeliveryIsReturnedForEach()
    {
        // Arrange (two onProgrammes for the same standard and agreement either side of a break in learning)
        var testFixture = new GetFm36QueryTestFixture(TestScenario.ApprenticeshipWithBreakInLearning);

        // Act
        await testFixture.CallSubjectUnderTest(collectionYear: AcademicYearWithEarnings);

        // Assert
        var learner = testFixture.Result.Items.Single();
        learner.LearningDeliveries.Select(x => x.AimSeqNumber).Should().BeEquivalentTo([1, 2]);
    }
}
