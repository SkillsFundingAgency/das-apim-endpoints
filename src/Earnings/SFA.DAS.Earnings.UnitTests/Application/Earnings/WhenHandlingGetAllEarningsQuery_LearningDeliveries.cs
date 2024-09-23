using FluentAssertions;

namespace SFA.DAS.Earnings.UnitTests.Application.Earnings;

public class WhenHandlingGetAllEarningsQuery_LearningDeliveries
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
    public void ThenALearningDeliveryIsCreatedForEachApprenticeship()
    {
        // Assert
        _testFixture.Result.Should().NotBeNull();

        _testFixture.Result.FM36Learners.Length.Should().Be(_testFixture.ApprenticeshipsResponse.Apprenticeships.Count);
        _testFixture.Result.FM36Learners.SelectMany(learner => learner.LearningDeliveries).Count().Should().Be(_testFixture.ApprenticeshipsResponse.Apprenticeships.Count);
    }

    [Test]
    public void ThenReturnsLearningDeliveryValuesForEachApprenticeship()
    {
        // Assert
        _testFixture.Result.Should().NotBeNull();

        foreach (var apprenticeship in _testFixture.ApprenticeshipsResponse.Apprenticeships)
        {
            var expectedPriceEpisodeStartDate = apprenticeship.StartDate > _testFixture.CollectionCalendarResponse.StartDate ? apprenticeship.StartDate : _testFixture.CollectionCalendarResponse.StartDate;
            var expectedPriceEpisodeEndDate = apprenticeship.PlannedEndDate < _testFixture.CollectionCalendarResponse.EndDate ? apprenticeship.PlannedEndDate : _testFixture.CollectionCalendarResponse.EndDate;
            var earningApprenticeship = _testFixture.EarningsResponse.SingleOrDefault(x => x.Key == apprenticeship.Key);
            var earningEpisode = _testFixture.EarningsResponse.SingleOrDefault(x => x.Key == apprenticeship.Key).Episodes.Single();

            var learningDelivery = _testFixture.Result.FM36Learners.SingleOrDefault(learner => learner.ULN.ToString() == apprenticeship.Uln).LearningDeliveries.SingleOrDefault();
            learningDelivery.Should().NotBeNull();
            learningDelivery.AimSeqNumber.Should().Be(1);
            learningDelivery.LearningDeliveryValues.ActualDaysIL.Should().Be(0);
            learningDelivery.LearningDeliveryValues.ActualNumInstalm.Should().BeNull();
            learningDelivery.LearningDeliveryValues.AdjStartDate.Should().Be(apprenticeship.StartDate);
            learningDelivery.LearningDeliveryValues.AgeAtProgStart.Should().Be(apprenticeship.AgeAtStartOfApprenticeship);
            learningDelivery.LearningDeliveryValues.AppAdjLearnStartDate.Should().Be(apprenticeship.StartDate);
            learningDelivery.LearningDeliveryValues.AppAdjLearnStartDateMatchPathway.Should().Be(apprenticeship.StartDate);
            learningDelivery.LearningDeliveryValues.ApplicCompDate.Should().Be(new DateTime(9999, 9, 9));
            learningDelivery.LearningDeliveryValues.CombinedAdjProp.Should().Be(1);
            learningDelivery.LearningDeliveryValues.Completed.Should().BeFalse();
            learningDelivery.LearningDeliveryValues.FirstIncentiveThresholdDate.Should().BeNull();
            learningDelivery.LearningDeliveryValues.FundStart.Should().BeTrue();
            learningDelivery.LearningDeliveryValues.LDApplic1618FrameworkUpliftTotalActEarnings.Should().Be(0);
            learningDelivery.LearningDeliveryValues.LearnAimRef.Should().Be("ZPROG001");
            learningDelivery.LearningDeliveryValues.LearnStartDate.Should().Be(apprenticeship.StartDate);
            learningDelivery.LearningDeliveryValues.LearnDel1618AtStart.Should().Be(apprenticeship.AgeAtStartOfApprenticeship < 19);
            learningDelivery.LearningDeliveryValues.LearnDelAppAccDaysIL.Should().Be(1 + (expectedPriceEpisodeEndDate - apprenticeship.StartDate).Days);
            learningDelivery.LearningDeliveryValues.LearnDelApplicDisadvAmount.Should().Be(0);
            learningDelivery.LearningDeliveryValues.LearnDelApplicEmp1618Incentive.Should().Be(0);
            learningDelivery.LearningDeliveryValues.LearnDelApplicProv1618FrameworkUplift.Should().Be(0);
            learningDelivery.LearningDeliveryValues.LearnDelApplicProv1618Incentive.Should().Be(0);
            learningDelivery.LearningDeliveryValues.LearnDelAppPrevAccDaysIL.Should().Be(1 + (expectedPriceEpisodeEndDate - expectedPriceEpisodeStartDate).Days);
            learningDelivery.LearningDeliveryValues.LearnDelDisadAmount.Should().Be(0);
            learningDelivery.LearningDeliveryValues.LearnDelEligDisadvPayment.Should().BeFalse();
            learningDelivery.LearningDeliveryValues.LearnDelEmpIdFirstAdditionalPaymentThreshold.Should().BeNull();
            learningDelivery.LearningDeliveryValues.LearnDelEmpIdSecondAdditionalPaymentThreshold.Should().BeNull();
            learningDelivery.LearningDeliveryValues.LearnDelHistDaysThisApp.Should().Be((_testFixture.CollectionCalendarResponse.EndDate - apprenticeship.StartDate).Days);
            learningDelivery.LearningDeliveryValues.LearnDelHistProgEarnings.Should().Be(earningEpisode.Instalments.Sum(i => i.Amount));
            learningDelivery.LearningDeliveryValues.LearnDelInitialFundLineType.Should().Be(earningApprenticeship.FundingLineType);
            learningDelivery.LearningDeliveryValues.LearnDelMathEng.Should().BeFalse();
            learningDelivery.LearningDeliveryValues.LearnDelProgEarliestACT2Date.Should().BeNull();
            learningDelivery.LearningDeliveryValues.LearnDelNonLevyProcured.Should().BeFalse();
            learningDelivery.LearningDeliveryValues.MathEngAimValue.Should().Be(0);
            learningDelivery.LearningDeliveryValues.OutstandNumOnProgInstalm.Should().BeNull();

            var expectedPlannedTotalDays = 1 + (apprenticeship.PlannedEndDate - apprenticeship.StartDate).Days;
            var expectedPlannedOnProgInstalments = 0;
            for (var i = 0; i < expectedPlannedTotalDays; i++)
            {
                if (apprenticeship.StartDate.AddDays(i).Day == DateTime.DaysInMonth(
                        apprenticeship.StartDate.AddDays(i).Year, apprenticeship.StartDate.AddDays(i).Month))
                    expectedPlannedOnProgInstalments++;
            }

            learningDelivery.LearningDeliveryValues.PlannedNumOnProgInstalm.Should().Be(expectedPlannedOnProgInstalments);
            learningDelivery.LearningDeliveryValues.PlannedTotalDaysIL.Should().Be(expectedPlannedTotalDays);
            learningDelivery.LearningDeliveryValues.ProgType.Should().Be(25);
            learningDelivery.LearningDeliveryValues.PwayCode.Should().BeNull();
            learningDelivery.LearningDeliveryValues.SecondIncentiveThresholdDate.Should().BeNull();
            learningDelivery.LearningDeliveryValues.StdCode.Should().Be(int.Parse(apprenticeship.Episodes.Single().TrainingCode));
            learningDelivery.LearningDeliveryValues.ThresholdDays.Should().Be(42);
            learningDelivery.LearningDeliveryValues.LearnDelApplicCareLeaverIncentive.Should().Be(0);
            learningDelivery.LearningDeliveryValues.LearnDelHistDaysCareLeavers.Should().Be(0);
            learningDelivery.LearningDeliveryValues.LearnDelAccDaysILCareLeavers.Should().Be(0);
            learningDelivery.LearningDeliveryValues.LearnDelPrevAccDaysILCareLeavers.Should().Be(0);
            learningDelivery.LearningDeliveryValues.LearnDelLearnerAddPayThresholdDate.Should().BeNull();
            learningDelivery.LearningDeliveryValues.LearnDelRedCode.Should().Be(0);
            learningDelivery.LearningDeliveryValues.LearnDelRedStartDate.Should().Be(new DateTime(9999, 9, 9));
        }
    }

    [Test]
    public void ThenReturnsDefaultedLearningDeliveryPeriodisedValuesForEachApprenticeship()
    {
        // Assert
        _testFixture.Result.Should().NotBeNull();

        foreach (var apprenticeship in _testFixture.ApprenticeshipsResponse.Apprenticeships)
        {
            var learningDelivery = _testFixture.Result.FM36Learners.SingleOrDefault(learner => learner.ULN.ToString() == apprenticeship.Uln).LearningDeliveries.SingleOrDefault();
            learningDelivery.Should().NotBeNull();
            learningDelivery.LearningDeliveryPeriodisedValues.Should()
                .Contain(x => x.AttributeName == "DisadvFirstPayment" && x.AllValuesAreSetToZero());
            learningDelivery.LearningDeliveryPeriodisedValues.Should()
                .Contain(x => x.AttributeName == "DisadvSecondPayment" && x.AllValuesAreSetToZero());
            learningDelivery.LearningDeliveryPeriodisedValues.Should()
                .Contain(x => x.AttributeName == "LDApplic1618FrameworkUpliftBalancingPayment" && x.AllValuesAreSetToZero());
            learningDelivery.LearningDeliveryPeriodisedValues.Should()
                .Contain(x => x.AttributeName == "LDApplic1618FrameworkUpliftCompletionPayment" && x.AllValuesAreSetToZero());
            learningDelivery.LearningDeliveryPeriodisedValues.Should()
                .Contain(x => x.AttributeName == "LDApplic1618FrameworkUpliftOnProgPayment" && x.AllValuesAreSetToZero());
            learningDelivery.LearningDeliveryPeriodisedValues.Should()
                .Contain(x => x.AttributeName == "LearnDelFirstEmp1618Pay" && x.AllValuesAreSetToZero());
            learningDelivery.LearningDeliveryPeriodisedValues.Should()
                .Contain(x => x.AttributeName == "LearnDelFirstProv1618Pay" && x.AllValuesAreSetToZero());
            learningDelivery.LearningDeliveryPeriodisedValues.Should()
                .Contain(x => x.AttributeName == "LearnDelLearnAddPayment" && x.AllValuesAreSetToZero());
            learningDelivery.LearningDeliveryPeriodisedValues.Should()
                .Contain(x => x.AttributeName == "LearnDelLevyNonPayInd" && x.AllValuesAreSetToZero());
            learningDelivery.LearningDeliveryPeriodisedValues.Should()
                .Contain(x => x.AttributeName == "LearnDelSecondEmp1618Pay" && x.AllValuesAreSetToZero());
            learningDelivery.LearningDeliveryPeriodisedValues.Should()
                .Contain(x => x.AttributeName == "LearnDelSecondProv1618Pay" && x.AllValuesAreSetToZero());
            learningDelivery.LearningDeliveryPeriodisedValues.Should()
                .Contain(x => x.AttributeName == "LearnDelSEMContWaiver" && x.AllValuesAreSetToZero());
            learningDelivery.LearningDeliveryPeriodisedValues.Should()
                .Contain(x => x.AttributeName == "LearnDelESFAContribPct" && x.AllValuesAreSetTo(0.95m));
            learningDelivery.LearningDeliveryPeriodisedValues.Should()
                .Contain(x => x.AttributeName == "LearnSuppFund" && x.AllValuesAreSetToZero());
            learningDelivery.LearningDeliveryPeriodisedValues.Should()
                .Contain(x => x.AttributeName == "LearnSuppFundCash" && x.AllValuesAreSetToZero());
            learningDelivery.LearningDeliveryPeriodisedValues.Should()
                .Contain(x => x.AttributeName == "MathEngBalPayment" && x.AllValuesAreSetToZero());
            learningDelivery.LearningDeliveryPeriodisedValues.Should()
                .Contain(x => x.AttributeName == "MathEngOnProgPayment" && x.AllValuesAreSetToZero());
            learningDelivery.LearningDeliveryPeriodisedValues.Should()
                .Contain(x => x.AttributeName == "ProgrammeAimBalPayment" && x.AllValuesAreSetToZero());
            learningDelivery.LearningDeliveryPeriodisedValues.Should()
                .Contain(x => x.AttributeName == "ProgrammeAimCompletionPayment" && x.AllValuesAreSetToZero());
        }
    }

    [Test]
    public void ThenReturnsInstPerPeriodValuesForEachApprenticeship()
    {
        // Assert
        _testFixture.Result.Should().NotBeNull();

        foreach (var apprenticeship in _testFixture.ApprenticeshipsResponse.Apprenticeships)
        {
            var earningEpisode = _testFixture.EarningsResponse.SingleOrDefault(x => x.Key == apprenticeship.Key).Episodes.Single();
            var academicYearInstalments = earningEpisode.Instalments.Where(x => x.AcademicYear == short.Parse(_testFixture.CollectionCalendarResponse.AcademicYear)).ToList();

            var learningDelivery = _testFixture.Result.FM36Learners.SingleOrDefault(learner => learner.ULN.ToString() == apprenticeship.Uln).LearningDeliveries.SingleOrDefault();
            learningDelivery.Should().NotBeNull();
            learningDelivery.LearningDeliveryPeriodisedValues.Should().NotBeNull();
            var result = learningDelivery.LearningDeliveryPeriodisedValues.SingleOrDefault(x => x.AttributeName == "InstPerPeriod");
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

    [Test]
    public void ThenReturnsProgrammeAimOnProgPaymentValuesForEachApprenticeship()
    {
        // Assert
        _testFixture.Result.Should().NotBeNull();

        foreach (var apprenticeship in _testFixture.ApprenticeshipsResponse.Apprenticeships)
        {
            var earningEpisode = _testFixture.EarningsResponse.SingleOrDefault(x => x.Key == apprenticeship.Key).Episodes.Single();
            var academicYearInstalments = earningEpisode.Instalments.Where(x => x.AcademicYear == short.Parse(_testFixture.CollectionCalendarResponse.AcademicYear)).ToList();

            var learningDelivery = _testFixture.Result.FM36Learners.SingleOrDefault(learner => learner.ULN.ToString() == apprenticeship.Uln).LearningDeliveries.SingleOrDefault();
            learningDelivery.Should().NotBeNull();
            learningDelivery.LearningDeliveryPeriodisedValues.Should().NotBeNull();
            var result = learningDelivery.LearningDeliveryPeriodisedValues.SingleOrDefault(x => x.AttributeName == "ProgrammeAimOnProgPayment");
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

    [Test]
    public void ThenReturnsProgrammeAimProgFundIndMaxEmpContValuesForEachApprenticeship()
    {
        // Assert
        _testFixture.Result.Should().NotBeNull();

        foreach (var apprenticeship in _testFixture.ApprenticeshipsResponse.Apprenticeships)
        {
            var earningEpisode = _testFixture.EarningsResponse.SingleOrDefault(x => x.Key == apprenticeship.Key).Episodes.Single();
            var academicYearInstalments = earningEpisode.Instalments.Where(x => x.AcademicYear == short.Parse(_testFixture.CollectionCalendarResponse.AcademicYear)).ToList();

            var learningDelivery = _testFixture.Result.FM36Learners.SingleOrDefault(learner => learner.ULN.ToString() == apprenticeship.Uln).LearningDeliveries.SingleOrDefault();
            learningDelivery.Should().NotBeNull();
            learningDelivery.LearningDeliveryPeriodisedValues.Should().NotBeNull();
            var result = learningDelivery.LearningDeliveryPeriodisedValues.SingleOrDefault(x => x.AttributeName == "ProgrammeAimProgFundIndMaxEmpCont");
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

    [Test]
    public void ThenReturnsProgrammeAimProgFundIndMinCoInvestValuesForEachApprenticeship()
    {
        // Assert
        _testFixture.Result.Should().NotBeNull();

        foreach (var apprenticeship in _testFixture.ApprenticeshipsResponse.Apprenticeships)
        {
            var earningEpisode = _testFixture.EarningsResponse.SingleOrDefault(x => x.Key == apprenticeship.Key).Episodes.Single();
            var academicYearInstalments = earningEpisode.Instalments.Where(x => x.AcademicYear == short.Parse(_testFixture.CollectionCalendarResponse.AcademicYear)).ToList();

            var learningDelivery = _testFixture.Result.FM36Learners.SingleOrDefault(learner => learner.ULN.ToString() == apprenticeship.Uln).LearningDeliveries.SingleOrDefault();
            learningDelivery.Should().NotBeNull();
            learningDelivery.LearningDeliveryPeriodisedValues.Should().NotBeNull();
            var result = learningDelivery.LearningDeliveryPeriodisedValues.SingleOrDefault(x => x.AttributeName == "ProgrammeAimProgFundIndMinCoInvest");
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

    [Test]
    public void ThenReturnsProgrammeAimTotProgFundValuesForEachApprenticeship()
    {
        // Assert
        _testFixture.Result.Should().NotBeNull();

        foreach (var apprenticeship in _testFixture.ApprenticeshipsResponse.Apprenticeships)
        {
            var earningEpisode = _testFixture.EarningsResponse.SingleOrDefault(x => x.Key == apprenticeship.Key).Episodes.Single();
            var academicYearInstalments = earningEpisode.Instalments.Where(x => x.AcademicYear == short.Parse(_testFixture.CollectionCalendarResponse.AcademicYear)).ToList();

            var learningDelivery = _testFixture.Result.FM36Learners.SingleOrDefault(learner => learner.ULN.ToString() == apprenticeship.Uln).LearningDeliveries.SingleOrDefault();
            learningDelivery.Should().NotBeNull();
            learningDelivery.LearningDeliveryPeriodisedValues.Should().NotBeNull();
            var result = learningDelivery.LearningDeliveryPeriodisedValues.SingleOrDefault(x => x.AttributeName == "ProgrammeAimTotProgFund");
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

    [Test]
    public void ThenReturnsLearningDeliveryPeriodisedTextValuesForEachApprenticeship()
    {
        // Assert
        _testFixture.Result.Should().NotBeNull();

        foreach (var apprenticeship in _testFixture.ApprenticeshipsResponse.Apprenticeships)
        {
            var earningsApprenticeship = _testFixture.EarningsResponse.SingleOrDefault(x => x.Key == apprenticeship.Key);

            var learningDelivery = _testFixture.Result.FM36Learners.SingleOrDefault(learner => learner.ULN.ToString() == apprenticeship.Uln).LearningDeliveries.SingleOrDefault();
            learningDelivery.Should().NotBeNull();
            learningDelivery.LearningDeliveryPeriodisedTextValues.Should().NotBeNull();
            learningDelivery.LearningDeliveryPeriodisedTextValues.Should().Contain(x =>
                x.AttributeName == "FundLineType" && x.AllValuesAreSetTo(earningsApprenticeship.FundingLineType));
            learningDelivery.LearningDeliveryPeriodisedTextValues.Should().Contain(x =>
                x.AttributeName == "LearnDelContType" && x.AllValuesAreSetTo("ACT1"));
        }
    }
}