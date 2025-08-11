using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using FluentAssertions;
using SFA.DAS.Earnings.Application.Earnings;
using SFA.DAS.Earnings.Application.Extensions;
using SFA.DAS.Earnings.UnitTests.Application.Extensions;
using SFA.DAS.Earnings.UnitTests.MockDataGenerator;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;
using static SFA.DAS.Earnings.Application.Earnings.EarningsFM36Constants;

namespace SFA.DAS.Earnings.UnitTests.Application.Earnings;

public class WhenHandlingGetAllEarningsQuery_PriceEpisodes
{
    [TestCase(TestScenario.SimpleApprenticeship, 1)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange, 2)]
    public async Task ThenAPriceEpisodeIsCreatedForEachPrice(TestScenario scenario, int expectedPriceEpisodes)
    {
        // Arrange
        var testFixture = new GetAllEarningsQueryTestFixture(scenario);

        var apprenticeship = testFixture.LearningsResponse.Learnings.Single();

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        testFixture.Result.Should().NotBeNull();
        
        var fm36Learner = testFixture.Result.FM36Learners
            .SingleOrDefault(x => x.ULN == long.Parse(apprenticeship.Uln));

        fm36Learner.PriceEpisodes.Count.Should().Be(apprenticeship.Episodes.SelectMany(episode => episode.Prices).Count());
    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    public async Task ThenReturnsPriceEpisodeValuesForEachApprenticeshipPriceEpisode(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetAllEarningsQueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        testFixture.Result.Should().NotBeNull();

        var apprenticeship = testFixture.LearningsResponse.Learnings.Single();
        
        var fm36Learner = testFixture.Result.FM36Learners
            .SingleOrDefault(x => x.ULN == long.Parse(apprenticeship.Uln));

        var expectedPriceEpisodesSplitByAcademicYear =
            testFixture.GetExpectedPriceEpisodesSplitByAcademicYear(apprenticeship.Episodes).ToList();

        foreach (var episodePrice in expectedPriceEpisodesSplitByAcademicYear)
        {
            var earningApprenticeship = testFixture.EarningsResponse.SingleOrDefault(x => x.Key == apprenticeship.Key);
            var earningEpisode = earningApprenticeship.Episodes.Single();

            var instalmentsForPricePeriod = earningEpisode.Instalments.Where(x =>
                x.AcademicYear.GetDateTime(x.DeliveryPeriod) >= testFixture.CollectionCalendarResponse.StartDate &&
                x.AcademicYear.GetDateTime(x.DeliveryPeriod) <= testFixture.CollectionCalendarResponse.EndDate &&
                (x.EpisodePriceKey == episodePrice.Price.Key || x.EpisodePriceKey == Guid.Empty) // the guid.empty is to account for apprenticeships that were created before episodePriceKey was recorded
                ).ToList();

            var expectedEpisodeTotalEarnings = earningEpisode.Instalments
                .Where(x => x.EpisodePriceKey == episodePrice.Price.Key)
                .Sum(x => x.Amount);

            var actualPriceEpisode = fm36Learner.PriceEpisodes.SingleOrDefault(x =>
                x.PriceEpisodeValues.EpisodeStartDate == episodePrice.Price.StartDate);
            actualPriceEpisode.Should().NotBeNull();

            actualPriceEpisode.PriceEpisodeIdentifier.Should()
                    .Be($"25-{episodePrice.Episode.TrainingCode.Trim()}-{episodePrice.Price.StartDate:dd/MM/yyyy}");

            actualPriceEpisode.PriceEpisodeValues.TNP1.Should().Be(episodePrice.Price.TrainingPrice);
            actualPriceEpisode.PriceEpisodeValues.TNP2.Should().Be(episodePrice.Price.EndPointAssessmentPrice);
            actualPriceEpisode.PriceEpisodeValues.TNP3.Should().Be(0);
            actualPriceEpisode.PriceEpisodeValues.TNP4.Should().Be(0);
            actualPriceEpisode.PriceEpisodeValues.PriceEpisode1618FUBalValue.Should().Be(0);
            actualPriceEpisode.PriceEpisodeValues.PriceEpisodeApplic1618FrameworkUpliftCompElement.Should().Be(0);
            actualPriceEpisode.PriceEpisodeValues.PriceEpisode1618FrameworkUpliftTotPrevEarnings.Should().Be(0);
            actualPriceEpisode.PriceEpisodeValues.PriceEpisode1618FrameworkUpliftRemainingAmount.Should().Be(0);
            actualPriceEpisode.PriceEpisodeValues.PriceEpisode1618FUMonthInstValue.Should().Be(0);
            actualPriceEpisode.PriceEpisodeValues.PriceEpisode1618FUTotEarnings.Should().Be(0);
            actualPriceEpisode.PriceEpisodeValues.PriceEpisodeUpperBandLimit.Should().Be(episodePrice.Price.FundingBandMaximum);
            actualPriceEpisode.PriceEpisodeValues.PriceEpisodePlannedEndDate.Should().Be(apprenticeship.PlannedEndDate);
            actualPriceEpisode.PriceEpisodeValues.PriceEpisodeTotalTNPPrice.Should().Be(episodePrice.Price.TotalPrice);
            actualPriceEpisode.PriceEpisodeValues.PriceEpisodeUpperLimitAdjustment.Should().Be(0);

            actualPriceEpisode.PriceEpisodeValues.PriceEpisodePlannedInstalments.Should().Be(InstalmentHelper.GetNumberOfInstalmentsBetweenDates(episodePrice.Price.StartDate, apprenticeship.PlannedEndDate));
            
            actualPriceEpisode.PriceEpisodeValues.PriceEpisodeInstalmentsThisPeriod.Should()
                .Be(episodePrice.Price.StartDate 
                    <= testFixture.CollectionCalendarResponse.GetCensusDateForCollectionPeriod(testFixture.CollectionPeriod)
                    &&
                    testFixture.CollectionCalendarResponse.GetCensusDateForCollectionPeriod(testFixture.CollectionPeriod)
                    <= episodePrice.Price.EndDate
                    &&
                    earningEpisode.Instalments.Any(x =>
                    x.AcademicYear == short.Parse(testFixture.CollectionCalendarResponse.AcademicYear) &&
                    x.DeliveryPeriod == testFixture.CollectionPeriod)
                    ? 1 : 0);
            
            actualPriceEpisode.PriceEpisodeValues.PriceEpisodeCompletionElement.Should().Be(earningEpisode.CompletionPayment);
            actualPriceEpisode.PriceEpisodeValues.PriceEpisodePreviousEarnings.Should().Be(0);
            actualPriceEpisode.PriceEpisodeValues.PriceEpisodeInstalmentValue.Should().Be(instalmentsForPricePeriod.First().Amount);
            actualPriceEpisode.PriceEpisodeValues.PriceEpisodeOnProgPayment.Should().Be(0);
            actualPriceEpisode.PriceEpisodeValues.PriceEpisodeTotalEarnings.Should().Be(expectedEpisodeTotalEarnings);
            actualPriceEpisode.PriceEpisodeValues.PriceEpisodeBalanceValue.Should().Be(0);
            actualPriceEpisode.PriceEpisodeValues.PriceEpisodeBalancePayment.Should().Be(0);
            actualPriceEpisode.PriceEpisodeValues.PriceEpisodeCompleted.Should().Be(episodePrice.Price.EndDate < DateTime.Now);
            actualPriceEpisode.PriceEpisodeValues.PriceEpisodeCompletionPayment.Should().Be(0);

            var previousYearEarnings = earningApprenticeship.Episodes
                .SelectMany(x => x.Instalments)
                .Where(x => x.AcademicYear.IsEarlierThan(short.Parse(testFixture.CollectionCalendarResponse.AcademicYear)))
                .Sum(x => x.Amount);
            var previousPeriodEarnings = earningApprenticeship.Episodes
                .SelectMany(x => x.Instalments)
                .Where(x => 
                    x.AcademicYear == short.Parse(testFixture.CollectionCalendarResponse.AcademicYear)
                    && x.DeliveryPeriod < testFixture.CollectionPeriod)
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
            actualPriceEpisode.PriceEpisodeValues.PriceEpisodeContractType.Should().Be("Contract for services with the employer");
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

    [Test]
    public async Task WhenNoSubsequentPriceInYearThenPriceEpisodeActualEndDateAndPriceEpisodeActualInstalmentsAreNotSet()
    {
        // Arrange
        var testFixture = new GetAllEarningsQueryTestFixture(TestScenario.SimpleApprenticeship);

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        var expectedEpisodePrice = GetExpectedEpisodePrice(testFixture);
        var priceEpisode = GetPriceEpisode(testFixture, expectedEpisodePrice);

        var fm36Learner = testFixture.Result.FM36Learners
            .SingleOrDefault(x => x.ULN == long.Parse(testFixture.LearningsResponse.Learnings.First().Uln));

        var expectedPriceEpisodesSplitByAcademicYear =
            testFixture.GetExpectedPriceEpisodesSplitByAcademicYear(testFixture.LearningsResponse.Learnings.First().Episodes).ToList();

        var episodePrice = expectedPriceEpisodesSplitByAcademicYear.Single().Price;

        var actualPriceEpisode = fm36Learner.PriceEpisodes.SingleOrDefault(x =>
            x.PriceEpisodeValues.EpisodeStartDate == episodePrice.StartDate);

        actualPriceEpisode.PriceEpisodeValues.PriceEpisodeActualEndDate.Should().BeNull();
        actualPriceEpisode.PriceEpisodeValues.PriceEpisodeActualInstalments.Should().Be(0);
    }

    [Test]
    public async Task WhenSubsequentPriceInYearThenPriceEpisodeActualEndDateAndPriceEpisodeActualInstalmentsAreSet()
    {
        // Arrange
        var testFixture = new GetAllEarningsQueryTestFixture(TestScenario.ApprenticeshipWithPriceChange);

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        var expectedEpisodePrice = GetExpectedEpisodePrice(testFixture);
        var priceEpisode = GetPriceEpisode(testFixture, expectedEpisodePrice);

        var fm36Learner = testFixture.Result.FM36Learners
            .SingleOrDefault(x => x.ULN == long.Parse(testFixture.LearningsResponse.Learnings.First().Uln));

        var expectedPriceEpisodesSplitByAcademicYear =
            testFixture.GetExpectedPriceEpisodesSplitByAcademicYear(testFixture.LearningsResponse.Learnings.First().Episodes).ToList();

        var episodePrice = expectedPriceEpisodesSplitByAcademicYear.First().Price;

        var actualPriceEpisode = fm36Learner.PriceEpisodes.SingleOrDefault(x =>
            x.PriceEpisodeValues.EpisodeStartDate == episodePrice.StartDate);

        actualPriceEpisode.PriceEpisodeValues.PriceEpisodeActualEndDate.Should().Be(episodePrice.EndDate);
        actualPriceEpisode.PriceEpisodeValues.PriceEpisodeActualInstalments.Should().Be(12);
    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    public async Task ThenReturnsDefaultedPriceEpisodePeriodisedValuesForEachApprenticeshipPriceEpisode(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetAllEarningsQueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        testFixture.Result.Should().NotBeNull();

        var apprenticeship = testFixture.LearningsResponse.Learnings.Single();
        
        var fm36Learner = testFixture.Result.FM36Learners
            .SingleOrDefault(x => x.ULN == long.Parse(apprenticeship.Uln));

        foreach (var episodePrice in testFixture.GetExpectedPriceEpisodesSplitByAcademicYear(apprenticeship.Episodes))
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
                .Contain(x => x.AttributeName == "PriceEpisodeLevyNonPayInd" && x.AllValuesAreSetToZero());
            actualPriceEpisode.PriceEpisodePeriodisedValues.Should()
                .Contain(x => x.AttributeName == "PriceEpisodeSecondDisadvantagePayment" && x.AllValuesAreSetToZero());
            actualPriceEpisode.PriceEpisodePeriodisedValues.Should()
                .Contain(x => x.AttributeName == "PriceEpisodeLearnerAdditionalPayment" && x.AllValuesAreSetToZero());
            actualPriceEpisode.PriceEpisodePeriodisedValues.Should()
                .Contain(x => x.AttributeName == "PriceEpisodeESFAContribPct" && x.AllValuesAreSetTo(0.95m));
        }
    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    public async Task ThenReturnsExpectedPriceEpisodeLSFCashPeriodisedValuesForEachApprenticeshipPriceEpisode(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetAllEarningsQueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        testFixture.Result.Should().NotBeNull();

        var apprenticeship = testFixture.LearningsResponse.Learnings.Single();

        var fm36Learner = testFixture.Result.FM36Learners
            .SingleOrDefault(x => x.ULN == long.Parse(apprenticeship.Uln));

        foreach (var episodePrice in testFixture.GetExpectedPriceEpisodesSplitByAcademicYear(apprenticeship.Episodes))
        {
            var actualPriceEpisode = fm36Learner.PriceEpisodes.SingleOrDefault(x =>
                x.PriceEpisodeValues.EpisodeStartDate == episodePrice.Price.StartDate);
            actualPriceEpisode.Should().NotBeNull();

            var expectedLearningSupport = testFixture.EarningsResponse.First()
                .Episodes.First()
                .AdditionalPayments.Where(x =>
                    x.AdditionalPaymentType == EarningsFM36Constants.AdditionalPaymentsTypes.LearningSupport &&
                x.AcademicYear == short.Parse(testFixture.CollectionCalendarResponse.AcademicYear))
                .Where(x => x.DueDate >= episodePrice.Price.StartDate && x.DueDate <= episodePrice.Price.EndDate)
            .ToList();

            var result = actualPriceEpisode.PriceEpisodePeriodisedValues.Single(x => x.AttributeName == PeriodisedAttributes.PriceEpisodeLSFCash);
            result.Should().NotBeNull();
            result.Period1.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 1)?.Amount ?? 0);
            result.Period2.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 2)?.Amount ?? 0);
            result.Period3.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 3)?.Amount ?? 0);
            result.Period4.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 4)?.Amount ?? 0);
            result.Period5.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 5)?.Amount ?? 0);
            result.Period6.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 6)?.Amount ?? 0);
            result.Period7.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 7)?.Amount ?? 0);
            result.Period8.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 8)?.Amount ?? 0);
            result.Period9.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 9)?.Amount ?? 0);
            result.Period10.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 10)?.Amount ?? 0);
            result.Period11.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 11)?.Amount ?? 0);
            result.Period12.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 12)?.Amount ?? 0);
        }
    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    public async Task ThenReturnsPriceEpisodeInstalmentsThisPeriodValuesForEachApprenticeship(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetAllEarningsQueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        testFixture.Result.Should().NotBeNull();

        var apprenticeship = testFixture.LearningsResponse.Learnings.Single();
        
        var fm36Learner = testFixture.Result.FM36Learners
            .SingleOrDefault(x => x.ULN == long.Parse(apprenticeship.Uln));

        foreach (var episodePrice in testFixture.GetExpectedPriceEpisodesSplitByAcademicYear(apprenticeship.Episodes))
        {
            var actualPriceEpisode = fm36Learner.PriceEpisodes.SingleOrDefault(x =>
                x.PriceEpisodeValues.EpisodeStartDate == episodePrice.Price.StartDate);
            actualPriceEpisode.Should().NotBeNull();

            var earningEpisode = testFixture.EarningsResponse.SingleOrDefault(x => x.Key == apprenticeship.Key).Episodes.Single();

            var academicYearInstalments = earningEpisode.Instalments
                .Where(x => x.AcademicYear == short.Parse(testFixture.CollectionCalendarResponse.AcademicYear))
                .Where(x => x.EpisodePriceKey == episodePrice.Price.Key)
                .ToList();

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

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    public async Task ThenReturnsPriceEpisodeOnProgPaymentValuesForEachApprenticeship(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetAllEarningsQueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        testFixture.Result.Should().NotBeNull();

        var apprenticeship = testFixture.LearningsResponse.Learnings.Single();
        
        var fm36Learner = testFixture.Result.FM36Learners
            .SingleOrDefault(x => x.ULN == long.Parse(apprenticeship.Uln));

        var expectedPriceEpisodes = testFixture.GetExpectedPriceEpisodesSplitByAcademicYear(apprenticeship.Episodes).ToList();
        foreach (var episodePrice in expectedPriceEpisodes)
        {
            var actualPriceEpisode = fm36Learner.PriceEpisodes.SingleOrDefault(x =>
                x.PriceEpisodeValues.EpisodeStartDate == episodePrice.Price.StartDate);
            actualPriceEpisode.Should().NotBeNull();

            var earningEpisode = testFixture.EarningsResponse.SingleOrDefault(x => x.Key == apprenticeship.Key).Episodes.Single();

            var academicYearInstalments = earningEpisode.Instalments
                .Where(x => x.AcademicYear == short.Parse(testFixture.CollectionCalendarResponse.AcademicYear))
                .Where(x => x.EpisodePriceKey == episodePrice.Price.Key)
                .ToList();

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

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    public async Task ThenReturnsPriceEpisodeProgFundIndMaxEmpContValuesForEachApprenticeship(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetAllEarningsQueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        testFixture.Result.Should().NotBeNull();

        var apprenticeship = testFixture.LearningsResponse.Learnings.Single();
    
        var fm36Learner = testFixture.Result.FM36Learners
            .SingleOrDefault(x => x.ULN == long.Parse(apprenticeship.Uln));

        foreach (var episodePrice in testFixture.GetExpectedPriceEpisodesSplitByAcademicYear(apprenticeship.Episodes))
        {
            var actualPriceEpisode = fm36Learner.PriceEpisodes.SingleOrDefault(x =>
                x.PriceEpisodeValues.EpisodeStartDate == episodePrice.Price.StartDate);
            actualPriceEpisode.Should().NotBeNull();

            var earningEpisode = testFixture.EarningsResponse.SingleOrDefault(x => x.Key == apprenticeship.Key).Episodes.Single();

            var academicYearInstalments = earningEpisode.Instalments
                .Where(x => x.AcademicYear == short.Parse(testFixture.CollectionCalendarResponse.AcademicYear))
                .Where(x => x.EpisodePriceKey == episodePrice.Price.Key)
                .ToList();

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

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    public async Task ThenReturnsPriceEpisodeProgFundIndMinCoInvestValuesForEachApprenticeship(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetAllEarningsQueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        testFixture.Result.Should().NotBeNull();

        var apprenticeship = testFixture.LearningsResponse.Learnings.Single();
        
        var fm36Learner = testFixture.Result.FM36Learners
            .SingleOrDefault(x => x.ULN == long.Parse(apprenticeship.Uln));

        foreach (var episodePrice in testFixture.GetExpectedPriceEpisodesSplitByAcademicYear(apprenticeship.Episodes))
        {
            var actualPriceEpisode = fm36Learner.PriceEpisodes.SingleOrDefault(x =>
                x.PriceEpisodeValues.EpisodeStartDate == episodePrice.Price.StartDate);
            actualPriceEpisode.Should().NotBeNull();

            var earningEpisode = testFixture.EarningsResponse.SingleOrDefault(x => x.Key == apprenticeship.Key).Episodes.Single();

            var academicYearInstalments = earningEpisode.Instalments
                .Where(x => x.AcademicYear == short.Parse(testFixture.CollectionCalendarResponse.AcademicYear))
                .Where(x => x.EpisodePriceKey == episodePrice.Price.Key)
                .ToList();

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

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    public async Task ThenReturnsPriceEpisodeTotProgFundingValuesForEachApprenticeship(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetAllEarningsQueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        testFixture.Result.Should().NotBeNull();

        var apprenticeship = testFixture.LearningsResponse.Learnings.Single();
        
        var fm36Learner = testFixture.Result.FM36Learners
            .SingleOrDefault(x => x.ULN == long.Parse(apprenticeship.Uln));

        foreach (var episodePrice in testFixture.GetExpectedPriceEpisodesSplitByAcademicYear(apprenticeship.Episodes))
        {
            var actualPriceEpisode = fm36Learner.PriceEpisodes.SingleOrDefault(x =>
                x.PriceEpisodeValues.EpisodeStartDate == episodePrice.Price.StartDate);
            actualPriceEpisode.Should().NotBeNull();

            var earningEpisode = testFixture.EarningsResponse.SingleOrDefault(x => x.Key == apprenticeship.Key).Episodes.Single();

            var academicYearInstalments = earningEpisode.Instalments
                .Where(x => x.AcademicYear == short.Parse(testFixture.CollectionCalendarResponse.AcademicYear))
                .Where(x => x.EpisodePriceKey == episodePrice.Price.Key)
                .ToList();

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

    [TestCase("ProviderIncentive", "PriceEpisodeFirstProv1618Pay", 1)]
    [TestCase("ProviderIncentive", "PriceEpisodeSecondProv1618Pay", 2)]
    [TestCase("EmployerIncentive", "PriceEpisodeFirstEmp1618Pay", 1)]
    [TestCase("EmployerIncentive", "PriceEpisodeSecondEmp1618Pay", 2)]
    public async Task ThenReturnsProviderAndEmployerIncentiveValuesForEachApprenticeship(string incentiveType, string attributeName, int paymentNumber)
    {
        // Arrange
        var testFixture = new GetAllEarningsQueryTestFixture(TestScenario.AllData);

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        testFixture.Result.Should().NotBeNull();

        foreach (var apprenticeship in testFixture.LearningsResponse.Learnings)
        {
            var earningEpisode = testFixture.EarningsResponse.SingleOrDefault(x => x.Key == apprenticeship.Key).Episodes.Single();

            var allIncentives = earningEpisode.AdditionalPayments
                    .Where(x => x.AdditionalPaymentType == incentiveType)
                    .OrderBy(x => x.DueDate).ToList();

            var fm36Learner = testFixture.Result.FM36Learners
                .SingleOrDefault(x => x.ULN == long.Parse(apprenticeship.Uln));

            foreach (var episodePrice in testFixture.GetExpectedPriceEpisodesSplitByAcademicYear(apprenticeship.Episodes))
            {
                var actualPriceEpisode = fm36Learner.PriceEpisodes
                    .FirstOrDefault(x => x.PriceEpisodeValues.EpisodeStartDate == episodePrice.Price.StartDate);

                actualPriceEpisode.Should().NotBeNull();

                var expectedAdditionalPayment = allIncentives
                    .Skip(paymentNumber - 1)
                    .FirstOrDefault();

                if (expectedAdditionalPayment != null &&
                    (expectedAdditionalPayment.DueDate < episodePrice.Price.StartDate ||
                     expectedAdditionalPayment.DueDate > episodePrice.Price.EndDate))
                {
                        expectedAdditionalPayment = null;
                }

                var result = actualPriceEpisode.PriceEpisodePeriodisedValues.SingleOrDefault(x => x.AttributeName == attributeName);
                result.Should().NotBeNull();
                result.Period1.Should().Be(expectedAdditionalPayment?.DeliveryPeriod == 1 ? expectedAdditionalPayment.Amount : 0);
                result.Period2.Should().Be(expectedAdditionalPayment?.DeliveryPeriod == 2 ? expectedAdditionalPayment.Amount : 0);
                result.Period3.Should().Be(expectedAdditionalPayment?.DeliveryPeriod == 3 ? expectedAdditionalPayment.Amount : 0);
                result.Period4.Should().Be(expectedAdditionalPayment?.DeliveryPeriod == 4 ? expectedAdditionalPayment.Amount : 0);
                result.Period5.Should().Be(expectedAdditionalPayment?.DeliveryPeriod == 5 ? expectedAdditionalPayment.Amount : 0);
                result.Period6.Should().Be(expectedAdditionalPayment?.DeliveryPeriod == 6 ? expectedAdditionalPayment.Amount : 0);
                result.Period7.Should().Be(expectedAdditionalPayment?.DeliveryPeriod == 7 ? expectedAdditionalPayment.Amount : 0);
                result.Period8.Should().Be(expectedAdditionalPayment?.DeliveryPeriod == 8 ? expectedAdditionalPayment.Amount : 0);
                result.Period9.Should().Be(expectedAdditionalPayment?.DeliveryPeriod == 9 ? expectedAdditionalPayment.Amount : 0);
                result.Period10.Should().Be(expectedAdditionalPayment?.DeliveryPeriod == 10 ? expectedAdditionalPayment.Amount : 0);
                result.Period11.Should().Be(expectedAdditionalPayment?.DeliveryPeriod == 11 ? expectedAdditionalPayment.Amount : 0);
                result.Period12.Should().Be(expectedAdditionalPayment?.DeliveryPeriod == 12 ? expectedAdditionalPayment.Amount : 0);
            }
        }
    }

    public enum ExpectedActualEndDate { IsNull, IsSameAsWithdrawalDate, IsDayBeforeNextPriceEpisode }

    [TestCase(ExpectedActualEndDate.IsNull, WithdrawalDate.None, TestScenario.SimpleApprenticeship)]
    [TestCase(ExpectedActualEndDate.IsSameAsWithdrawalDate, WithdrawalDate.AfterQualifyingPeriod, TestScenario.SimpleApprenticeship)]
    [TestCase(ExpectedActualEndDate.IsSameAsWithdrawalDate, WithdrawalDate.DuringQualifyingPeriod, TestScenario.SimpleApprenticeship)]
    [TestCase(ExpectedActualEndDate.IsDayBeforeNextPriceEpisode, WithdrawalDate.None, TestScenario.ApprenticeshipWithPriceChange)]
    [TestCase(ExpectedActualEndDate.IsSameAsWithdrawalDate, WithdrawalDate.BeforeNextPriceEpisodeStart, TestScenario.ApprenticeshipWithPriceChange)]
    [TestCase(ExpectedActualEndDate.IsDayBeforeNextPriceEpisode, WithdrawalDate.AfterNextPriceEpisodeStart, TestScenario.ApprenticeshipWithPriceChange)]
    public async Task ThenPriceEpisodeActualEndDateMatchesExpectations(ExpectedActualEndDate expectedActualEndDate, WithdrawalDate withdrawalDate, TestScenario testScenario)
    {
        // Arrange
        var testFixture = new GetAllEarningsQueryTestFixture(testScenario);
        var withdrawDate = testFixture.LearningsResponse.Learnings.First().SetWithdrawalDate(withdrawalDate);

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        var expectedEpisodePrice = GetExpectedEpisodePrice(testFixture);
        var priceEpisode = GetPriceEpisode(testFixture, expectedEpisodePrice);

        switch(expectedActualEndDate)
        {
            case ExpectedActualEndDate.IsNull:
                priceEpisode.PriceEpisodeValues.PriceEpisodeActualEndDate.Should().BeNull();
                break;
            case ExpectedActualEndDate.IsSameAsWithdrawalDate:
                priceEpisode.PriceEpisodeValues.PriceEpisodeActualEndDate.Should().Be(withdrawDate);
                break;
            case ExpectedActualEndDate.IsDayBeforeNextPriceEpisode:
                var nextPriceStartDate = expectedEpisodePrice.EndDate.AddDays(1);
                priceEpisode.PriceEpisodeValues.PriceEpisodeActualEndDate.Should().Be(nextPriceStartDate.AddDays(-1));
                break;
        }
    }

    public enum ExpectedPriceEpisodeActualEndDateIncEPA { IsNull, IsCompletionDate, IsNextPriceEpisodeStartDate }
    public enum SetCompletionDateTo { Null, BeforeEndOfCurrentEpisode, AfterStartOfNextEpisode }

    [TestCase(ExpectedPriceEpisodeActualEndDateIncEPA.IsNull, SetCompletionDateTo.Null, TestScenario.SimpleApprenticeship)]
    [TestCase(ExpectedPriceEpisodeActualEndDateIncEPA.IsCompletionDate, SetCompletionDateTo.BeforeEndOfCurrentEpisode, TestScenario.SimpleApprenticeship)]
    [TestCase(ExpectedPriceEpisodeActualEndDateIncEPA.IsNextPriceEpisodeStartDate, SetCompletionDateTo.Null, TestScenario.ApprenticeshipWithPriceChange)]
    [TestCase(ExpectedPriceEpisodeActualEndDateIncEPA.IsCompletionDate, SetCompletionDateTo.BeforeEndOfCurrentEpisode, TestScenario.ApprenticeshipWithPriceChange)]
    [TestCase(ExpectedPriceEpisodeActualEndDateIncEPA.IsNextPriceEpisodeStartDate, SetCompletionDateTo.AfterStartOfNextEpisode, TestScenario.ApprenticeshipWithPriceChange)]
    public async Task ThenPriceEpisodeActualEndDateIncEPAMatchesExpectations(ExpectedPriceEpisodeActualEndDateIncEPA expectedDate, SetCompletionDateTo setCompletionDateTo, TestScenario testScenario)
    {
        // Arrange
        var testFixture = new GetAllEarningsQueryTestFixture(testScenario);
        var apprenticeship = testFixture.LearningsResponse.Learnings.First();
        var prices = apprenticeship.Episodes.First().Prices.OrderBy(x=>x.StartDate);

        switch (setCompletionDateTo)
        {
            case SetCompletionDateTo.Null:
                apprenticeship.CompletionDate = null;
                break;

            case SetCompletionDateTo.BeforeEndOfCurrentEpisode:
                apprenticeship.CompletionDate = prices.First().EndDate.AddDays(-5);
                break;

            case SetCompletionDateTo.AfterStartOfNextEpisode:
                apprenticeship.CompletionDate = prices.ElementAt(1).StartDate.AddDays(5);
                break;
        }


        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        var expectedEpisodePrice = GetExpectedEpisodePrice(testFixture);
        var priceEpisode = GetPriceEpisode(testFixture, expectedEpisodePrice);

        switch (expectedDate)
        {
            case ExpectedPriceEpisodeActualEndDateIncEPA.IsNull:
                priceEpisode.PriceEpisodeValues.PriceEpisodeActualEndDateIncEPA.Should().BeNull();
                break;
            case ExpectedPriceEpisodeActualEndDateIncEPA.IsCompletionDate:
                priceEpisode.PriceEpisodeValues.PriceEpisodeActualEndDateIncEPA.Should().Be(apprenticeship.CompletionDate);
                break;
            case ExpectedPriceEpisodeActualEndDateIncEPA.IsNextPriceEpisodeStartDate:
                priceEpisode.PriceEpisodeValues.PriceEpisodeActualEndDateIncEPA.Should().Be(expectedEpisodePrice.EndDate.AddDays(1));
                break;
        }
    }

    [Test]
    public async Task ThenPeriodisedValuesPriceEpisodeBalancePaymentMatchesExpectations()
    {
        // Arrange
        var testFixture = new GetAllEarningsQueryTestFixture(TestScenario.SimpleApprenticeship);
        var instalments = testFixture.EarningsResponse.First().Episodes.First().Instalments;
        var balancingPayment = new Instalment
        {
            AcademicYear = short.Parse(testFixture.CollectionCalendarResponse.AcademicYear),
            Amount = 1000,
            DeliveryPeriod = 6,
            EpisodePriceKey = instalments.First().EpisodePriceKey,
            InstalmentType = InstalmentType.Balancing.ToString()
        };
        instalments.Add(balancingPayment);

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        var expectedEpisodePrice = GetExpectedEpisodePrice(testFixture);
        var priceEpisode = GetPriceEpisode(testFixture, expectedEpisodePrice);
        var balancingPaymentPeriodisedValues = GetPriceEpisodePeriodisedValues(priceEpisode, PeriodisedAttributes.PriceEpisodeBalancePayment);
        balancingPaymentPeriodisedValues.HasExactPeriodValues([0, 0, 0, 0, 0, 1000, 0, 0, 0, 0, 0, 0]).Should().BeTrue();

    }

    [Test]
    public async Task ThenPeriodisedValuesPriceEpisodeCompletionPaymentMatchesExpectations()
    {
        // Arrange
        var testFixture = new GetAllEarningsQueryTestFixture(TestScenario.SimpleApprenticeship);
        var instalments = testFixture.EarningsResponse.First().Episodes.First().Instalments;
        var completionPayment = new Instalment
        {
            AcademicYear = short.Parse(testFixture.CollectionCalendarResponse.AcademicYear),
            Amount = 1000,
            DeliveryPeriod = 6,
            EpisodePriceKey = instalments.First().EpisodePriceKey,
            InstalmentType = InstalmentType.Completion.ToString()
        };
        instalments.Add(completionPayment);

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        var expectedEpisodePrice = GetExpectedEpisodePrice(testFixture);
        var priceEpisode = GetPriceEpisode(testFixture, expectedEpisodePrice);
        var completionPaymentPeriodisedValues = GetPriceEpisodePeriodisedValues(priceEpisode, PeriodisedAttributes.PriceEpisodeCompletionPayment);
        completionPaymentPeriodisedValues.HasExactPeriodValues([0, 0, 0, 0, 0, 1000, 0, 0, 0, 0, 0, 0]).Should().BeTrue();

    }

    private static EpisodePrice GetExpectedEpisodePrice(GetAllEarningsQueryTestFixture testFixture)
    {
        testFixture.Result.Should().NotBeNull();

        var expectedPriceEpisodesSplitByAcademicYear =
            testFixture.GetExpectedPriceEpisodesSplitByAcademicYear(testFixture.LearningsResponse.Learnings.First().Episodes).ToList();

        var episodePrice = expectedPriceEpisodesSplitByAcademicYear.First().Price;

        return episodePrice;
    }

    private static PriceEpisode GetPriceEpisode(GetAllEarningsQueryTestFixture testFixture, EpisodePrice expectedEpisodePrice)
    {
        testFixture.Result.Should().NotBeNull();

        var fm36Learner = testFixture.Result.FM36Learners
            .SingleOrDefault(x => x.ULN == long.Parse(testFixture.LearningsResponse.Learnings.First().Uln));

        var actualPriceEpisode = fm36Learner.PriceEpisodes.SingleOrDefault(x =>
            x.PriceEpisodeValues.EpisodeStartDate == expectedEpisodePrice.StartDate);

        return actualPriceEpisode;
    }

    private static PriceEpisodePeriodisedValues GetPriceEpisodePeriodisedValues(PriceEpisode priceEpisode, string valueName)
    {
        return priceEpisode.PriceEpisodePeriodisedValues
            .SingleOrDefault(x => x.AttributeName == valueName)
            ?? throw new Exception($"Price episode periodised values for {valueName} not found.");
    }
}
