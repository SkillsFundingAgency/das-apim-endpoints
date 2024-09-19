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

    [Test]
    public void ThenReturnsPriceEpisodeValuesForEachApprenticeshipPriceEpisode()
    {
        // Assert
        _testFixture.Result.Should().NotBeNull();

        foreach (var apprenticeship in _testFixture.ApprenticeshipsResponse.Apprenticeships)
        {
            var fm36Learner = _testFixture.Result.FM36Learners
                .SingleOrDefault(x => x.ULN == long.Parse(apprenticeship.Uln));

            foreach (var episodePrice in _testFixture.GetExpectedPriceEpisodesSplitByAcademicYear(apprenticeship.Episodes))
            {
                var earningApprenticeship = _testFixture.EarningsResponse.SingleOrDefault(x => x.Key == apprenticeship.Key);
                var earningEpisode = earningApprenticeship.Episodes.Single();

                var actualPriceEpisode = fm36Learner.PriceEpisodes.SingleOrDefault(x =>
                    x.PriceEpisodeValues.EpisodeStartDate == episodePrice.Price.StartDate);
                actualPriceEpisode.Should().NotBeNull();

                actualPriceEpisode.PriceEpisodeIdentifier.Should()
                    .Be($"25-{episodePrice.Episode.TrainingCode.Trim()}-{episodePrice.Price.StartDate:dd/MM/yyyy}");

                actualPriceEpisode.PriceEpisodeValues.TNP1.Should().Be(episodePrice.Price.TrainingPrice);
                actualPriceEpisode.PriceEpisodeValues.TNP2.Should().Be(episodePrice.Price.EndPointAssessmentPrice);
                actualPriceEpisode.PriceEpisodeValues.TNP3.Should().Be(0);
                actualPriceEpisode.PriceEpisodeValues.TNP4.Should().Be(0);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeActualEndDateIncEPA.Should().BeNull();
                actualPriceEpisode.PriceEpisodeValues.PriceEpisode1618FUBalValue.Should().Be(0);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeApplic1618FrameworkUpliftCompElement.Should().Be(0);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisode1618FrameworkUpliftTotPrevEarnings.Should().Be(0);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisode1618FrameworkUpliftRemainingAmount.Should().Be(0);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisode1618FUMonthInstValue.Should().Be(0);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisode1618FUTotEarnings.Should().Be(0);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeUpperBandLimit.Should().Be(episodePrice.Price.FundingBandMaximum);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodePlannedEndDate.Should().Be(apprenticeship.PlannedEndDate);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeActualEndDate.Should().Be(episodePrice.Price.EndDate);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeTotalTNPPrice.Should().Be(episodePrice.Price.TotalPrice);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeUpperLimitAdjustment.Should().Be(0);

                var expectedPlannedTotalDays = (apprenticeship.PlannedEndDate - episodePrice.Price.StartDate).Days;
                var expectedPlannedInstalments = 0;
                for (var i = 0; i < expectedPlannedTotalDays; i++)
                {
                    if (apprenticeship.StartDate.AddDays(i).Day == DateTime.DaysInMonth(
                            apprenticeship.StartDate.AddDays(i).Year, apprenticeship.StartDate.AddDays(i).Month))
                        expectedPlannedInstalments++;
                }

                actualPriceEpisode.PriceEpisodeValues.PriceEpisodePlannedInstalments.Should().Be(expectedPlannedInstalments);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeActualInstalments.Should()
                    .Be(earningEpisode.Instalments.Count(x => x.AcademicYear == short.Parse(_testFixture.CollectionCalendarResponse.AcademicYear)));
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeInstalmentsThisPeriod.Should()
                    .Be(earningEpisode.Instalments.Any(x =>
                        x.AcademicYear == short.Parse(_testFixture.CollectionCalendarResponse.AcademicYear) &&
                        x.DeliveryPeriod == _testFixture.CollectionPeriod)
                        ? 1 : 0);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeCompletionElement.Should().Be(earningEpisode.CompletionPayment);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodePreviousEarnings.Should().Be(0);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeInstalmentValue.Should().Be(earningEpisode.Instalments.First().Amount);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeOnProgPayment.Should().Be(0);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeTotalEarnings.Should().Be(earningEpisode.Instalments.Sum(x => x.Amount));
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeBalanceValue.Should().Be(0);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeBalancePayment.Should().Be(0);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeCompleted.Should().Be(episodePrice.Price.EndDate < DateTime.Now);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeCompletionPayment.Should().Be(0);

                var previousYearEarnings = earningApprenticeship.Episodes
                    .SelectMany(x => x.Instalments)
                    .Where(x => x.AcademicYear.IsEarlierThan(short.Parse(_testFixture.CollectionCalendarResponse.AcademicYear)))
                    .Sum(x => x.Amount);
                var previousPeriodEarnings = earningApprenticeship.Episodes
                    .SelectMany(x => x.Instalments)
                    .Where(x => 
                        x.AcademicYear == short.Parse(_testFixture.CollectionCalendarResponse.AcademicYear)
                        && x.DeliveryPeriod < _testFixture.CollectionPeriod)
                    .Sum(x => x.Amount);
                var allPreviousEarnings = previousYearEarnings + previousPeriodEarnings;

                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeRemainingTNPAmount.Should().Be(episodePrice.Price.FundingBandMaximum - allPreviousEarnings);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeRemainingAmountWithinUpperLimit.Should().Be(episodePrice.Price.FundingBandMaximum - allPreviousEarnings);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeCappedRemainingTNPAmount.Should().Be(episodePrice.Price.FundingBandMaximum - allPreviousEarnings);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeExpectedTotalMonthlyValue.Should().Be(episodePrice.Price.FundingBandMaximum - allPreviousEarnings - earningEpisode.CompletionPayment);

                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeAimSeqNumber.Should().Be(1);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeFirstDisadvantagePayment.Should().Be(0);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeSecondDisadvantagePayment.Should().Be(0);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeApplic1618FrameworkUpliftBalancing.Should().Be(0);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeApplic1618FrameworkUpliftCompletionPayment.Should().Be(0);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeApplic1618FrameworkUpliftOnProgPayment.Should().Be(0);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeSecondProv1618Pay.Should().Be(0);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeFirstEmp1618Pay.Should().Be(0);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeSecondEmp1618Pay.Should().Be(0);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeFirstProv1618Pay.Should().Be(0);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeLSFCash.Should().Be(0);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeFundLineType.Should().Be(earningApprenticeship.FundingLineType);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeLevyNonPayInd.Should().Be(0);
                actualPriceEpisode.PriceEpisodeValues.EpisodeEffectiveTNPStartDate.Should().Be(episodePrice.Price.StartDate);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeFirstAdditionalPaymentThresholdDate.Should().BeNull();
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeSecondAdditionalPaymentThresholdDate.Should().BeNull();
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeContractType.Should().Be("ACT1");
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodePreviousEarningsSameProvider.Should().Be(0);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeTotProgFunding.Should().Be(earningEpisode.OnProgramTotal);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeProgFundIndMinCoInvest.Should().Be(earningEpisode.OnProgramTotal * 0.95m);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeProgFundIndMaxEmpCont.Should().Be(earningEpisode.OnProgramTotal * 0.05m);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeTotalPMRs.Should().Be(0);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeCumulativePMRs.Should().Be(0);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeCompExemCode.Should().Be(0);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeLearnerAdditionalPaymentThresholdDate.Should().BeNull();
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeRedStartDate.Should().BeNull();
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeRedStatusCode.Should().Be(0);
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeLDAppIdent.Should().Be($"25-{episodePrice.Episode.TrainingCode.Trim()}");
                actualPriceEpisode.PriceEpisodeValues.PriceEpisodeAugmentedBandLimitFactor.Should().Be(1);
            }
            fm36Learner.PriceEpisodes.Count.Should().Be(apprenticeship.Episodes.SelectMany(episode => episode.Prices).Count());
        }
    }

    [Test]
    public void ThenReturnsDefaultedPriceEpisodePeriodisedValuesForEachApprenticeshipPriceEpisode()
    {
        // Assert
        _testFixture.Result.Should().NotBeNull();

        foreach (var apprenticeship in _testFixture.ApprenticeshipsResponse.Apprenticeships)
        {
            var fm36Learner = _testFixture.Result.FM36Learners
                .SingleOrDefault(x => x.ULN == long.Parse(apprenticeship.Uln));

            foreach (var episodePrice in _testFixture.GetExpectedPriceEpisodesSplitByAcademicYear(apprenticeship.Episodes))
            {
                var actualPriceEpisode = fm36Learner.PriceEpisodes.SingleOrDefault(x =>
                    x.PriceEpisodeValues.EpisodeStartDate == episodePrice.Price.StartDate);
                actualPriceEpisode.Should().NotBeNull();

                actualPriceEpisode.PriceEpisodePeriodisedValues.Should()
                    .Contain(x => x.AttributeName == "PriceEpisodeApplic1618FrameworkUpliftBalancing" && x.AllValuesAreSetToZero());
                actualPriceEpisode.PriceEpisodePeriodisedValues.Should()
                    .Contain(x => x.AttributeName == "PriceEpisodeApplic1618FrameworkUpliftCompletionPayment" && x.AllValuesAreSetToZero());
                actualPriceEpisode.PriceEpisodePeriodisedValues.Should()
                    .Contain(x => x.AttributeName == "PriceEpisodeApplic1618FrameworkUpliftOnProgPayment" && x.AllValuesAreSetToZero());
                actualPriceEpisode.PriceEpisodePeriodisedValues.Should()
                    .Contain(x => x.AttributeName == "PriceEpisodeBalancePayment" && x.AllValuesAreSetToZero());
                actualPriceEpisode.PriceEpisodePeriodisedValues.Should()
                    .Contain(x => x.AttributeName == "PriceEpisodeBalanceValue" && x.AllValuesAreSetToZero());
                actualPriceEpisode.PriceEpisodePeriodisedValues.Should()
                    .Contain(x => x.AttributeName == "PriceEpisodeCompletionPayment" && x.AllValuesAreSetToZero());
                actualPriceEpisode.PriceEpisodePeriodisedValues.Should()
                    .Contain(x => x.AttributeName == "PriceEpisodeFirstDisadvantagePayment" && x.AllValuesAreSetToZero());
                actualPriceEpisode.PriceEpisodePeriodisedValues.Should()
                    .Contain(x => x.AttributeName == "PriceEpisodeFirstEmp1618Pay" && x.AllValuesAreSetToZero());
                actualPriceEpisode.PriceEpisodePeriodisedValues.Should()
                    .Contain(x => x.AttributeName == "PriceEpisodeFirstProv1618Pay" && x.AllValuesAreSetToZero());
                actualPriceEpisode.PriceEpisodePeriodisedValues.Should()
                    .Contain(x => x.AttributeName == "PriceEpisodeLevyNonPayInd" && x.AllValuesAreSetToZero());
                actualPriceEpisode.PriceEpisodePeriodisedValues.Should()
                    .Contain(x => x.AttributeName == "PriceEpisodeLSFCash" && x.AllValuesAreSetToZero());
                actualPriceEpisode.PriceEpisodePeriodisedValues.Should()
                    .Contain(x => x.AttributeName == "PriceEpisodeSecondDisadvantagePayment" && x.AllValuesAreSetToZero());
                actualPriceEpisode.PriceEpisodePeriodisedValues.Should()
                    .Contain(x => x.AttributeName == "PriceEpisodeSecondEmp1618Pay" && x.AllValuesAreSetToZero());
                actualPriceEpisode.PriceEpisodePeriodisedValues.Should()
                    .Contain(x => x.AttributeName == "PriceEpisodeSecondProv1618Pay" && x.AllValuesAreSetToZero());
                actualPriceEpisode.PriceEpisodePeriodisedValues.Should()
                    .Contain(x => x.AttributeName == "PriceEpisodeLearnerAdditionalPayment" && x.AllValuesAreSetToZero());
            }
        }
    }

    [Test]
    public void ThenReturnsPriceEpisodeInstalmentsThisPeriodValuesForEachApprenticeship()
    {
        // Assert
        _testFixture.Result.Should().NotBeNull();

        foreach (var apprenticeship in _testFixture.ApprenticeshipsResponse.Apprenticeships)
        {
            var fm36Learner = _testFixture.Result.FM36Learners
                .SingleOrDefault(x => x.ULN == long.Parse(apprenticeship.Uln));

            foreach (var episodePrice in _testFixture.GetExpectedPriceEpisodesSplitByAcademicYear(apprenticeship.Episodes))
            {
                var actualPriceEpisode = fm36Learner.PriceEpisodes.SingleOrDefault(x =>
                    x.PriceEpisodeValues.EpisodeStartDate == episodePrice.Price.StartDate);
                actualPriceEpisode.Should().NotBeNull();

                var earningEpisode = _testFixture.EarningsResponse.SingleOrDefault(x => x.Key == apprenticeship.Key).Episodes.Single();
                var academicYearInstalments = earningEpisode.Instalments.Where(x => x.AcademicYear == short.Parse(_testFixture.CollectionCalendarResponse.AcademicYear)).ToList();

                var result = actualPriceEpisode.PriceEpisodePeriodisedValues.SingleOrDefault(x => x.AttributeName == "PriceEpisodeInstalmentsThisPeriod");
                result.Should().NotBeNull();
                result.Period1.Should().Be(academicYearInstalments.Any(i => i.DeliveryPeriod == 1 && i.Amount != 0) ? 1 : 0);
                result.Period2.Should().Be(academicYearInstalments.Any(i => i.DeliveryPeriod == 2 && i.Amount != 0) ? 1 : 0);
                result.Period3.Should().Be(academicYearInstalments.Any(i => i.DeliveryPeriod == 3 && i.Amount != 0) ? 1 : 0);
                result.Period4.Should().Be(academicYearInstalments.Any(i => i.DeliveryPeriod == 4 && i.Amount != 0) ? 1 : 0);
                result.Period5.Should().Be(academicYearInstalments.Any(i => i.DeliveryPeriod == 5 && i.Amount != 0) ? 1 : 0);
                result.Period6.Should().Be(academicYearInstalments.Any(i => i.DeliveryPeriod == 6 && i.Amount != 0) ? 1 : 0);
                result.Period7.Should().Be(academicYearInstalments.Any(i => i.DeliveryPeriod == 7 && i.Amount != 0) ? 1 : 0);
                result.Period8.Should().Be(academicYearInstalments.Any(i => i.DeliveryPeriod == 8 && i.Amount != 0) ? 1 : 0);
                result.Period9.Should().Be(academicYearInstalments.Any(i => i.DeliveryPeriod == 9 && i.Amount != 0) ? 1 : 0);
                result.Period10.Should().Be(academicYearInstalments.Any(i => i.DeliveryPeriod == 10 && i.Amount != 0) ? 1 : 0);
                result.Period11.Should().Be(academicYearInstalments.Any(i => i.DeliveryPeriod == 11 && i.Amount != 0) ? 1 : 0);
                result.Period12.Should().Be(academicYearInstalments.Any(i => i.DeliveryPeriod == 12 && i.Amount != 0) ? 1 : 0);
            }
        }
    }

    [Test]
    public void ThenReturnsPriceEpisodeOnProgPaymentValuesForEachApprenticeship()
    {
        // Assert
        _testFixture.Result.Should().NotBeNull();

        foreach (var apprenticeship in _testFixture.ApprenticeshipsResponse.Apprenticeships)
        {
            var fm36Learner = _testFixture.Result.FM36Learners
                .SingleOrDefault(x => x.ULN == long.Parse(apprenticeship.Uln));

            foreach (var episodePrice in _testFixture.GetExpectedPriceEpisodesSplitByAcademicYear(apprenticeship.Episodes))
            {
                var actualPriceEpisode = fm36Learner.PriceEpisodes.SingleOrDefault(x =>
                    x.PriceEpisodeValues.EpisodeStartDate == episodePrice.Price.StartDate);
                actualPriceEpisode.Should().NotBeNull();

                var earningEpisode = _testFixture.EarningsResponse.SingleOrDefault(x => x.Key == apprenticeship.Key).Episodes.Single();
                var academicYearInstalments = earningEpisode.Instalments.Where(x => x.AcademicYear == short.Parse(_testFixture.CollectionCalendarResponse.AcademicYear)).ToList();

                var result = actualPriceEpisode.PriceEpisodePeriodisedValues.SingleOrDefault(x => x.AttributeName == "PriceEpisodeOnProgPayment");
                result.Should().NotBeNull();
                result.Period1.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 1)?.Amount).GetValueOrDefault());
                result.Period2.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 2)?.Amount).GetValueOrDefault());
                result.Period3.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 3)?.Amount).GetValueOrDefault());
                result.Period4.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 4)?.Amount).GetValueOrDefault());
                result.Period5.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 5)?.Amount).GetValueOrDefault());
                result.Period6.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 6)?.Amount).GetValueOrDefault());
                result.Period7.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 7)?.Amount).GetValueOrDefault());
                result.Period8.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 8)?.Amount).GetValueOrDefault());
                result.Period9.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 9)?.Amount).GetValueOrDefault());
                result.Period10.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 10)?.Amount).GetValueOrDefault());
                result.Period11.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 11)?.Amount).GetValueOrDefault());
                result.Period12.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 12)?.Amount).GetValueOrDefault());
            }
        }
    }

    [Test]
    public void ThenReturnsPriceEpisodeProgFundIndMaxEmpContValuesForEachApprenticeship()
    {
        // Assert
        _testFixture.Result.Should().NotBeNull();

        foreach (var apprenticeship in _testFixture.ApprenticeshipsResponse.Apprenticeships)
        {
            var fm36Learner = _testFixture.Result.FM36Learners
                .SingleOrDefault(x => x.ULN == long.Parse(apprenticeship.Uln));

            foreach (var episodePrice in _testFixture.GetExpectedPriceEpisodesSplitByAcademicYear(apprenticeship.Episodes))
            {
                var actualPriceEpisode = fm36Learner.PriceEpisodes.SingleOrDefault(x =>
                    x.PriceEpisodeValues.EpisodeStartDate == episodePrice.Price.StartDate);
                actualPriceEpisode.Should().NotBeNull();

                var earningEpisode = _testFixture.EarningsResponse.SingleOrDefault(x => x.Key == apprenticeship.Key).Episodes.Single();
                var academicYearInstalments = earningEpisode.Instalments.Where(x => x.AcademicYear == short.Parse(_testFixture.CollectionCalendarResponse.AcademicYear)).ToList();

                var result = actualPriceEpisode.PriceEpisodePeriodisedValues.SingleOrDefault(x => x.AttributeName == "PriceEpisodeProgFundIndMaxEmpCont");
                result.Should().NotBeNull();

                var expectedCoInvestEmployerPercentage = 0.05m;

                result.Period1.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 1)?.Amount * expectedCoInvestEmployerPercentage).GetValueOrDefault());
                result.Period2.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 2)?.Amount * expectedCoInvestEmployerPercentage).GetValueOrDefault());
                result.Period3.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 3)?.Amount * expectedCoInvestEmployerPercentage).GetValueOrDefault());
                result.Period4.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 4)?.Amount * expectedCoInvestEmployerPercentage).GetValueOrDefault());
                result.Period5.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 5)?.Amount * expectedCoInvestEmployerPercentage).GetValueOrDefault());
                result.Period6.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 6)?.Amount * expectedCoInvestEmployerPercentage).GetValueOrDefault());
                result.Period7.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 7)?.Amount * expectedCoInvestEmployerPercentage).GetValueOrDefault());
                result.Period8.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 8)?.Amount * expectedCoInvestEmployerPercentage).GetValueOrDefault());
                result.Period9.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 9)?.Amount * expectedCoInvestEmployerPercentage).GetValueOrDefault());
                result.Period10.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 10)?.Amount * expectedCoInvestEmployerPercentage).GetValueOrDefault());
                result.Period11.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 11)?.Amount * expectedCoInvestEmployerPercentage).GetValueOrDefault());
                result.Period12.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 12)?.Amount * expectedCoInvestEmployerPercentage).GetValueOrDefault());
            }
        }
    }

    [Test]
    public void ThenReturnsPriceEpisodeProgFundIndMinCoInvestValuesForEachApprenticeship()
    {
        // Assert
        _testFixture.Result.Should().NotBeNull();

        foreach (var apprenticeship in _testFixture.ApprenticeshipsResponse.Apprenticeships)
        {
            var fm36Learner = _testFixture.Result.FM36Learners
                .SingleOrDefault(x => x.ULN == long.Parse(apprenticeship.Uln));

            foreach (var episodePrice in _testFixture.GetExpectedPriceEpisodesSplitByAcademicYear(apprenticeship.Episodes))
            {
                var actualPriceEpisode = fm36Learner.PriceEpisodes.SingleOrDefault(x =>
                    x.PriceEpisodeValues.EpisodeStartDate == episodePrice.Price.StartDate);
                actualPriceEpisode.Should().NotBeNull();

                var earningEpisode = _testFixture.EarningsResponse.SingleOrDefault(x => x.Key == apprenticeship.Key).Episodes.Single();
                var academicYearInstalments = earningEpisode.Instalments.Where(x => x.AcademicYear == short.Parse(_testFixture.CollectionCalendarResponse.AcademicYear)).ToList();

                var result = actualPriceEpisode.PriceEpisodePeriodisedValues.SingleOrDefault(x => x.AttributeName == "PriceEpisodeProgFundIndMinCoInvest");
                result.Should().NotBeNull();

                var expectedCoInvestSfaPercentage = 0.95m;

                result.Period1.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 1)?.Amount * expectedCoInvestSfaPercentage).GetValueOrDefault());
                result.Period2.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 2)?.Amount * expectedCoInvestSfaPercentage).GetValueOrDefault());
                result.Period3.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 3)?.Amount * expectedCoInvestSfaPercentage).GetValueOrDefault());
                result.Period4.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 4)?.Amount * expectedCoInvestSfaPercentage).GetValueOrDefault());
                result.Period5.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 5)?.Amount * expectedCoInvestSfaPercentage).GetValueOrDefault());
                result.Period6.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 6)?.Amount * expectedCoInvestSfaPercentage).GetValueOrDefault());
                result.Period7.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 7)?.Amount * expectedCoInvestSfaPercentage).GetValueOrDefault());
                result.Period8.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 8)?.Amount * expectedCoInvestSfaPercentage).GetValueOrDefault());
                result.Period9.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 9)?.Amount * expectedCoInvestSfaPercentage).GetValueOrDefault());
                result.Period10.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 10)?.Amount * expectedCoInvestSfaPercentage).GetValueOrDefault());
                result.Period11.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 11)?.Amount * expectedCoInvestSfaPercentage).GetValueOrDefault());
                result.Period12.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 12)?.Amount * expectedCoInvestSfaPercentage).GetValueOrDefault());
            }
        }
    }

    [Test]
    public void ThenReturnsPriceEpisodeTotProgFundingValuesForEachApprenticeship()
    {
        // Assert
        _testFixture.Result.Should().NotBeNull();

        foreach (var apprenticeship in _testFixture.ApprenticeshipsResponse.Apprenticeships)
        {
            var fm36Learner = _testFixture.Result.FM36Learners
                .SingleOrDefault(x => x.ULN == long.Parse(apprenticeship.Uln));

            foreach (var episodePrice in _testFixture.GetExpectedPriceEpisodesSplitByAcademicYear(apprenticeship.Episodes))
            {
                var actualPriceEpisode = fm36Learner.PriceEpisodes.SingleOrDefault(x =>
                    x.PriceEpisodeValues.EpisodeStartDate == episodePrice.Price.StartDate);
                actualPriceEpisode.Should().NotBeNull();

                var earningEpisode = _testFixture.EarningsResponse.SingleOrDefault(x => x.Key == apprenticeship.Key).Episodes.Single();
                var academicYearInstalments = earningEpisode.Instalments.Where(x => x.AcademicYear == short.Parse(_testFixture.CollectionCalendarResponse.AcademicYear)).ToList();

                var result = actualPriceEpisode.PriceEpisodePeriodisedValues.SingleOrDefault(x => x.AttributeName == "PriceEpisodeTotProgFunding");
                result.Should().NotBeNull();
                result.Period1.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 1)?.Amount).GetValueOrDefault());
                result.Period2.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 2)?.Amount).GetValueOrDefault());
                result.Period3.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 3)?.Amount).GetValueOrDefault());
                result.Period4.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 4)?.Amount).GetValueOrDefault());
                result.Period5.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 5)?.Amount).GetValueOrDefault());
                result.Period6.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 6)?.Amount).GetValueOrDefault());
                result.Period7.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 7)?.Amount).GetValueOrDefault());
                result.Period8.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 8)?.Amount).GetValueOrDefault());
                result.Period9.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 9)?.Amount).GetValueOrDefault());
                result.Period10.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 10)?.Amount).GetValueOrDefault());
                result.Period11.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 11)?.Amount).GetValueOrDefault());
                result.Period12.Should().Be((academicYearInstalments.SingleOrDefault(i => i.DeliveryPeriod == 12)?.Amount).GetValueOrDefault());
            }
        }
    }
}