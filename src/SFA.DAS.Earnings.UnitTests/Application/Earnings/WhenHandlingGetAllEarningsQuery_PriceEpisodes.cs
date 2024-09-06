using FluentAssertions;

namespace SFA.DAS.Earnings.UnitTests.Application.Earnings;

public class WhenHandlingGetAllEarningsQuery_PriceEpisodes
{
    private GetAllEarningsQueryTestFixture _testFixture;

    [SetUp]
    public async Task SetUp()
    {
        // Arrange
        _testFixture = new GetAllEarningsQueryTestFixture();

        // Act
        await _testFixture.CallSubjectUnderTest();
    }

    [Test]
    public void ThenReturnsPriceEpisodeValues()
    {
        // Assert
        _testFixture.Result.Should().NotBeNull();

        foreach (var apprenticeship in _testFixture.ApprenticeshipsResponse.Apprenticeships)
        {
            var fm36Learner = _testFixture.Result.FM36Learners.SingleOrDefault(x => x.ULN == long.Parse(apprenticeship.Uln));
            foreach (var apprenticeshipEpisode in apprenticeship.Episodes)
            {
                foreach (var apprenticeshipEpisodePrice in apprenticeshipEpisode.Prices)
                {
                    fm36Learner.PriceEpisodes.Should().Contain(episode =>
                        episode.PriceEpisodeIdentifier == $"25-{apprenticeshipEpisode.TrainingCode}-{apprenticeshipEpisodePrice.StartDate:dd/MM/yyyy}"
                        && episode.PriceEpisodeValues.EpisodeStartDate == apprenticeshipEpisodePrice.StartDate);
                }
            }
        }
    }

    [Test]
    public void ThenAPriceEpisodeIsCreatedForEachPrice()
    {
        // Assert
        _testFixture.Result.Should().NotBeNull();

        foreach (var apprenticeship in _testFixture.ApprenticeshipsResponse.Apprenticeships)
        {
            var fm36Learner = _testFixture.Result.FM36Learners
                .SingleOrDefault(x => x.ULN == long.Parse(apprenticeship.Uln));

            fm36Learner.PriceEpisodes.Count.Should().Be(apprenticeship.Episodes.SelectMany(episode => episode.Prices).Count());
        }
    }
}