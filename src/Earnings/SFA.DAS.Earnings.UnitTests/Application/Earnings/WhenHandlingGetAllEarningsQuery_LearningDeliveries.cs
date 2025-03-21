using FluentAssertions;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;

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
            var firstAdditionalPaymentDueDate =
                earningEpisode.AdditionalPayments.OrderBy(x => x.DueDate).First().DueDate;
            var expectedFirstIncentiveThresholdDate =
                firstAdditionalPaymentDueDate >= apprenticeship.StartDate &&
                firstAdditionalPaymentDueDate <= apprenticeship.PlannedEndDate
                    ? firstAdditionalPaymentDueDate
                    : (DateTime?)null;
            learningDelivery.LearningDeliveryValues.FirstIncentiveThresholdDate.Should().Be(expectedFirstIncentiveThresholdDate);
            learningDelivery.LearningDeliveryValues.LDApplic1618FrameworkUpliftTotalActEarnings.Should().Be(0);
            learningDelivery.LearningDeliveryValues.LearnAimRef.Should().Be("ZPROG001");
            learningDelivery.LearningDeliveryValues.LearnStartDate.Should().Be(apprenticeship.StartDate);
            learningDelivery.LearningDeliveryValues.LearnDel1618AtStart.Should().Be(apprenticeship.AgeAtStartOfApprenticeship < 19);
            learningDelivery.LearningDeliveryValues.LearnDelAppAccDaysIL.Should().Be(1 + (expectedPriceEpisodeEndDate - apprenticeship.StartDate).Days);
            learningDelivery.LearningDeliveryValues.LearnDelApplicDisadvAmount.Should().Be(0);
            learningDelivery.LearningDeliveryValues.LearnDelApplicEmp1618Incentive.Should().Be(earningEpisode.AdditionalPayments.Where(x => x.AdditionalPaymentType == "EmployerIncentive").Sum(x => x.Amount));
            learningDelivery.LearningDeliveryValues.LearnDelApplicProv1618FrameworkUplift.Should().Be(0);
            learningDelivery.LearningDeliveryValues.LearnDelApplicProv1618Incentive.Should().Be(earningEpisode.AdditionalPayments.Where(x => x.AdditionalPaymentType == "ProviderIncentive").Sum(x => x.Amount));
            learningDelivery.LearningDeliveryValues.LearnDelAppPrevAccDaysIL.Should().Be(1 + (expectedPriceEpisodeEndDate - expectedPriceEpisodeStartDate).Days);
            learningDelivery.LearningDeliveryValues.LearnDelDisadAmount.Should().Be(0);
            learningDelivery.LearningDeliveryValues.LearnDelEligDisadvPayment.Should().BeFalse();
            learningDelivery.LearningDeliveryValues.LearnDelEmpIdFirstAdditionalPaymentThreshold.Should().BeNull();
            learningDelivery.LearningDeliveryValues.LearnDelEmpIdSecondAdditionalPaymentThreshold.Should().BeNull();
            learningDelivery.LearningDeliveryValues.LearnDelHistDaysThisApp.Should().Be(1 + (_testFixture.CollectionCalendarResponse.EndDate - apprenticeship.StartDate).Days);
            learningDelivery.LearningDeliveryValues.LearnDelHistProgEarnings.Should().Be(earningEpisode.Instalments.Sum(i => i.Amount));
            learningDelivery.LearningDeliveryValues.LearnDelInitialFundLineType.Should().Be(earningApprenticeship.FundingLineType);
            learningDelivery.LearningDeliveryValues.LearnDelMathEng.Should().BeFalse();
            learningDelivery.LearningDeliveryValues.LearnDelProgEarliestACT2Date.Should().BeNull();
            learningDelivery.LearningDeliveryValues.LearnDelNonLevyProcured.Should().BeFalse();
            learningDelivery.LearningDeliveryValues.MathEngAimValue.Should().Be(0);
            learningDelivery.LearningDeliveryValues.OutstandNumOnProgInstalm.Should().BeNull();

            var expectedPlannedTotalDays = 1 + (apprenticeship.PlannedEndDate - apprenticeship.StartDate).Days;

            learningDelivery.LearningDeliveryValues.PlannedNumOnProgInstalm.Should().Be(InstalmentHelper.GetNumberOfInstalmentsBetweenDates(apprenticeship.StartDate, apprenticeship.PlannedEndDate));
            learningDelivery.LearningDeliveryValues.PlannedTotalDaysIL.Should().Be(expectedPlannedTotalDays);
            learningDelivery.LearningDeliveryValues.ProgType.Should().Be(25);
            learningDelivery.LearningDeliveryValues.PwayCode.Should().BeNull();
            var secondAdditionalPaymentDueDate =
                earningEpisode.AdditionalPayments
                    .DistinctBy(x => x.DueDate)
                    .OrderBy(x => x.DueDate)
                    .Skip(1)
                    .FirstOrDefault()?
                    .DueDate;
            var expectedSecondIncentiveThresholdDate =
                secondAdditionalPaymentDueDate >= apprenticeship.StartDate &&
                secondAdditionalPaymentDueDate <= apprenticeship.PlannedEndDate
                    ? secondAdditionalPaymentDueDate
                    : (DateTime?)null;
            learningDelivery.LearningDeliveryValues.SecondIncentiveThresholdDate.Should().Be(expectedSecondIncentiveThresholdDate);
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
                .Contain(x => x.AttributeName == "LearnDelLearnAddPayment" && x.AllValuesAreSetToZero());
            learningDelivery.LearningDeliveryPeriodisedValues.Should()
                .Contain(x => x.AttributeName == "LearnDelLevyNonPayInd" && x.AllValuesAreSetToZero());
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
                x.AttributeName == "LearnDelContType" && x.AllValuesAreSetTo("Contract for services with the employer"));
        }
    }

    [Test]
    public void ThenReturnsLearningDeliveryPeriodisedLearnDelFirstProv1618PayValuesForEachApprenticeship()
    {
        // Assert
        _testFixture.Result.Should().NotBeNull();

        foreach (var apprenticeship in _testFixture.ApprenticeshipsResponse.Apprenticeships)
        {
            var earningEpisode = _testFixture.EarningsResponse.SingleOrDefault(x => x.Key == apprenticeship.Key).Episodes.Single();
            var providerIncentives = earningEpisode.AdditionalPayments.Where(x => x.AdditionalPaymentType == "ProviderIncentive").ToList();

            var learningDelivery = _testFixture.Result.FM36Learners.SingleOrDefault(learner => learner.ULN.ToString() == apprenticeship.Uln).LearningDeliveries.SingleOrDefault();
            learningDelivery.Should().NotBeNull();
            learningDelivery.LearningDeliveryPeriodisedValues.Should().NotBeNull();
            var result = learningDelivery.LearningDeliveryPeriodisedValues.SingleOrDefault(x => x.AttributeName == "LearnDelFirstProv1618Pay");
            result.Should().NotBeNull();
            var expectedLearnDelFirstProv1618Pay = providerIncentives.Where(x => x.AcademicYear.ToString() == _testFixture.CollectionCalendarResponse.AcademicYear).MinBy(x => x.DeliveryPeriod);
            
            result.Period1.Should().Be((expectedLearnDelFirstProv1618Pay?.DeliveryPeriod == 1 ? expectedLearnDelFirstProv1618Pay.Amount : 0));
            result.Period2.Should().Be((expectedLearnDelFirstProv1618Pay?.DeliveryPeriod == 2 ? expectedLearnDelFirstProv1618Pay.Amount : 0));
            result.Period3.Should().Be((expectedLearnDelFirstProv1618Pay?.DeliveryPeriod == 3 ? expectedLearnDelFirstProv1618Pay.Amount : 0));
            result.Period4.Should().Be((expectedLearnDelFirstProv1618Pay?.DeliveryPeriod == 4 ? expectedLearnDelFirstProv1618Pay.Amount : 0));
            result.Period5.Should().Be((expectedLearnDelFirstProv1618Pay?.DeliveryPeriod == 5 ? expectedLearnDelFirstProv1618Pay.Amount : 0));
            result.Period6.Should().Be((expectedLearnDelFirstProv1618Pay?.DeliveryPeriod == 6 ? expectedLearnDelFirstProv1618Pay.Amount : 0));
            result.Period7.Should().Be((expectedLearnDelFirstProv1618Pay?.DeliveryPeriod == 7 ? expectedLearnDelFirstProv1618Pay.Amount : 0));
            result.Period8.Should().Be((expectedLearnDelFirstProv1618Pay?.DeliveryPeriod == 8 ? expectedLearnDelFirstProv1618Pay.Amount : 0));
            result.Period9.Should().Be((expectedLearnDelFirstProv1618Pay?.DeliveryPeriod == 9 ? expectedLearnDelFirstProv1618Pay.Amount : 0));
            result.Period10.Should().Be((expectedLearnDelFirstProv1618Pay?.DeliveryPeriod == 10 ? expectedLearnDelFirstProv1618Pay.Amount : 0));
            result.Period11.Should().Be((expectedLearnDelFirstProv1618Pay?.DeliveryPeriod == 11 ? expectedLearnDelFirstProv1618Pay.Amount : 0));
            result.Period12.Should().Be((expectedLearnDelFirstProv1618Pay?.DeliveryPeriod == 12 ? expectedLearnDelFirstProv1618Pay.Amount : 0));
        }
    }

    [Test]
    public void ThenReturnsLearningDeliveryPeriodisedLearnDelSecondProv1618PayValuesForEachApprenticeship()
    {
        // Assert
        _testFixture.Result.Should().NotBeNull();

        foreach (var apprenticeship in _testFixture.ApprenticeshipsResponse.Apprenticeships)
        {
            var earningEpisode = _testFixture.EarningsResponse.SingleOrDefault(x => x.Key == apprenticeship.Key).Episodes.Single();
            var providerIncentives = earningEpisode.AdditionalPayments.Where(x => x.AdditionalPaymentType == "ProviderIncentive").ToList();

            var learningDelivery = _testFixture.Result.FM36Learners.SingleOrDefault(learner => learner.ULN.ToString() == apprenticeship.Uln).LearningDeliveries.SingleOrDefault();
            learningDelivery.Should().NotBeNull();
            learningDelivery.LearningDeliveryPeriodisedValues.Should().NotBeNull();
            var result = learningDelivery.LearningDeliveryPeriodisedValues.SingleOrDefault(x => x.AttributeName == "LearnDelSecondProv1618Pay");
            result.Should().NotBeNull();
            var expectedLearnDelSecondProv1618Pay = providerIncentives.Where(x => x.AcademicYear.ToString() == _testFixture.CollectionCalendarResponse.AcademicYear).OrderBy(x => x.DeliveryPeriod).Skip(1).FirstOrDefault();

            result.Period1.Should().Be((expectedLearnDelSecondProv1618Pay?.DeliveryPeriod == 1 ? expectedLearnDelSecondProv1618Pay.Amount : 0));
            result.Period2.Should().Be((expectedLearnDelSecondProv1618Pay?.DeliveryPeriod == 2 ? expectedLearnDelSecondProv1618Pay.Amount : 0));
            result.Period3.Should().Be((expectedLearnDelSecondProv1618Pay?.DeliveryPeriod == 3 ? expectedLearnDelSecondProv1618Pay.Amount : 0));
            result.Period4.Should().Be((expectedLearnDelSecondProv1618Pay?.DeliveryPeriod == 4 ? expectedLearnDelSecondProv1618Pay.Amount : 0));
            result.Period5.Should().Be((expectedLearnDelSecondProv1618Pay?.DeliveryPeriod == 5 ? expectedLearnDelSecondProv1618Pay.Amount : 0));
            result.Period6.Should().Be((expectedLearnDelSecondProv1618Pay?.DeliveryPeriod == 6 ? expectedLearnDelSecondProv1618Pay.Amount : 0));
            result.Period7.Should().Be((expectedLearnDelSecondProv1618Pay?.DeliveryPeriod == 7 ? expectedLearnDelSecondProv1618Pay.Amount : 0));
            result.Period8.Should().Be((expectedLearnDelSecondProv1618Pay?.DeliveryPeriod == 8 ? expectedLearnDelSecondProv1618Pay.Amount : 0));
            result.Period9.Should().Be((expectedLearnDelSecondProv1618Pay?.DeliveryPeriod == 9 ? expectedLearnDelSecondProv1618Pay.Amount : 0));
            result.Period10.Should().Be((expectedLearnDelSecondProv1618Pay?.DeliveryPeriod == 10 ? expectedLearnDelSecondProv1618Pay.Amount : 0));
            result.Period11.Should().Be((expectedLearnDelSecondProv1618Pay?.DeliveryPeriod == 11 ? expectedLearnDelSecondProv1618Pay.Amount : 0));
            result.Period12.Should().Be((expectedLearnDelSecondProv1618Pay?.DeliveryPeriod == 12 ? expectedLearnDelSecondProv1618Pay.Amount : 0));
        }
    }

    [Test]
    public void ThenReturnsLearningDeliveryPeriodisedLearnDelFirstEmp1618PayValuesForEachApprenticeship()
    {
        // Assert
        _testFixture.Result.Should().NotBeNull();

        foreach (var apprenticeship in _testFixture.ApprenticeshipsResponse.Apprenticeships)
        {
            var earningEpisode = _testFixture.EarningsResponse.SingleOrDefault(x => x.Key == apprenticeship.Key).Episodes.Single();
            var employerIncentives = earningEpisode.AdditionalPayments.Where(x => x.AdditionalPaymentType == "EmployerIncentive").ToList();

            var learningDelivery = _testFixture.Result.FM36Learners.SingleOrDefault(learner => learner.ULN.ToString() == apprenticeship.Uln).LearningDeliveries.SingleOrDefault();
            learningDelivery.Should().NotBeNull();
            learningDelivery.LearningDeliveryPeriodisedValues.Should().NotBeNull();
            var result = learningDelivery.LearningDeliveryPeriodisedValues.SingleOrDefault(x => x.AttributeName == "LearnDelFirstEmp1618Pay");
            result.Should().NotBeNull();
            var expectedLearnDelFirstEmp1618Pay = employerIncentives.Where(x => x.AcademicYear.ToString() == _testFixture.CollectionCalendarResponse.AcademicYear).MinBy(x => x.DeliveryPeriod);

            result.Period1.Should().Be((expectedLearnDelFirstEmp1618Pay?.DeliveryPeriod == 1 ? expectedLearnDelFirstEmp1618Pay.Amount : 0));
            result.Period2.Should().Be((expectedLearnDelFirstEmp1618Pay?.DeliveryPeriod == 2 ? expectedLearnDelFirstEmp1618Pay.Amount : 0));
            result.Period3.Should().Be((expectedLearnDelFirstEmp1618Pay?.DeliveryPeriod == 3 ? expectedLearnDelFirstEmp1618Pay.Amount : 0));
            result.Period4.Should().Be((expectedLearnDelFirstEmp1618Pay?.DeliveryPeriod == 4 ? expectedLearnDelFirstEmp1618Pay.Amount : 0));
            result.Period5.Should().Be((expectedLearnDelFirstEmp1618Pay?.DeliveryPeriod == 5 ? expectedLearnDelFirstEmp1618Pay.Amount : 0));
            result.Period6.Should().Be((expectedLearnDelFirstEmp1618Pay?.DeliveryPeriod == 6 ? expectedLearnDelFirstEmp1618Pay.Amount : 0));
            result.Period7.Should().Be((expectedLearnDelFirstEmp1618Pay?.DeliveryPeriod == 7 ? expectedLearnDelFirstEmp1618Pay.Amount : 0));
            result.Period8.Should().Be((expectedLearnDelFirstEmp1618Pay?.DeliveryPeriod == 8 ? expectedLearnDelFirstEmp1618Pay.Amount : 0));
            result.Period9.Should().Be((expectedLearnDelFirstEmp1618Pay?.DeliveryPeriod == 9 ? expectedLearnDelFirstEmp1618Pay.Amount : 0));
            result.Period10.Should().Be((expectedLearnDelFirstEmp1618Pay?.DeliveryPeriod == 10 ? expectedLearnDelFirstEmp1618Pay.Amount : 0));
            result.Period11.Should().Be((expectedLearnDelFirstEmp1618Pay?.DeliveryPeriod == 11 ? expectedLearnDelFirstEmp1618Pay.Amount : 0));
            result.Period12.Should().Be((expectedLearnDelFirstEmp1618Pay?.DeliveryPeriod == 12 ? expectedLearnDelFirstEmp1618Pay.Amount : 0));
        }
    }

    [Test]
    public void ThenReturnsLearningDeliveryPeriodisedLearnDelSecondEmp1618PayValuesForEachApprenticeship()
    {
        // Assert
        _testFixture.Result.Should().NotBeNull();

        foreach (var apprenticeship in _testFixture.ApprenticeshipsResponse.Apprenticeships)
        {
            var earningEpisode = _testFixture.EarningsResponse.SingleOrDefault(x => x.Key == apprenticeship.Key).Episodes.Single();
            var employerIncentives = earningEpisode.AdditionalPayments.Where(x => x.AdditionalPaymentType == "EmployerIncentive").ToList();

            var learningDelivery = _testFixture.Result.FM36Learners.SingleOrDefault(learner => learner.ULN.ToString() == apprenticeship.Uln).LearningDeliveries.SingleOrDefault();
            learningDelivery.Should().NotBeNull();
            learningDelivery.LearningDeliveryPeriodisedValues.Should().NotBeNull();
            var result = learningDelivery.LearningDeliveryPeriodisedValues.SingleOrDefault(x => x.AttributeName == "LearnDelSecondEmp1618Pay");
            result.Should().NotBeNull();
            var expectedLearnDelSecondEmp1618Pay = employerIncentives.Where(x => x.AcademicYear.ToString() == _testFixture.CollectionCalendarResponse.AcademicYear).OrderBy(x => x.DeliveryPeriod).Skip(1).FirstOrDefault();

            result.Period1.Should().Be((expectedLearnDelSecondEmp1618Pay?.DeliveryPeriod == 1 ? expectedLearnDelSecondEmp1618Pay.Amount : 0));
            result.Period2.Should().Be((expectedLearnDelSecondEmp1618Pay?.DeliveryPeriod == 2 ? expectedLearnDelSecondEmp1618Pay.Amount : 0));
            result.Period3.Should().Be((expectedLearnDelSecondEmp1618Pay?.DeliveryPeriod == 3 ? expectedLearnDelSecondEmp1618Pay.Amount : 0));
            result.Period4.Should().Be((expectedLearnDelSecondEmp1618Pay?.DeliveryPeriod == 4 ? expectedLearnDelSecondEmp1618Pay.Amount : 0));
            result.Period5.Should().Be((expectedLearnDelSecondEmp1618Pay?.DeliveryPeriod == 5 ? expectedLearnDelSecondEmp1618Pay.Amount : 0));
            result.Period6.Should().Be((expectedLearnDelSecondEmp1618Pay?.DeliveryPeriod == 6 ? expectedLearnDelSecondEmp1618Pay.Amount : 0));
            result.Period7.Should().Be((expectedLearnDelSecondEmp1618Pay?.DeliveryPeriod == 7 ? expectedLearnDelSecondEmp1618Pay.Amount : 0));
            result.Period8.Should().Be((expectedLearnDelSecondEmp1618Pay?.DeliveryPeriod == 8 ? expectedLearnDelSecondEmp1618Pay.Amount : 0));
            result.Period9.Should().Be((expectedLearnDelSecondEmp1618Pay?.DeliveryPeriod == 9 ? expectedLearnDelSecondEmp1618Pay.Amount : 0));
            result.Period10.Should().Be((expectedLearnDelSecondEmp1618Pay?.DeliveryPeriod == 10 ? expectedLearnDelSecondEmp1618Pay.Amount : 0));
            result.Period11.Should().Be((expectedLearnDelSecondEmp1618Pay?.DeliveryPeriod == 11 ? expectedLearnDelSecondEmp1618Pay.Amount : 0));
            result.Period12.Should().Be((expectedLearnDelSecondEmp1618Pay?.DeliveryPeriod == 12 ? expectedLearnDelSecondEmp1618Pay.Amount : 0));
        }
    }

    [TestCase("StillInLearning", true)]
    [TestCase("WithdrawnAfterQualifyingPeriod", true)]
    [TestCase("WithdrawnBeforeQualifyingPeriod", false)]
    public async Task ThenReturnsCorrectFundStartValueForApprenticeship(string status, bool expectedFundingStart)
    {
        // Arrange
        _testFixture.Result.Should().NotBeNull();
        string uln = string.Empty;
        _testFixture.EditApprenticeshipResponse(0, x => {
            switch (status)
            {
                case "StillInLearning":
                    x.WithdrawnDate = null;
                    break;
                case "WithdrawnAfterQualifyingPeriod":
                    x.WithdrawnDate = x.StartDate.AddDays(SFA.DAS.SharedOuterApi.Common.Constants.QualifyingPeriod + 1);
                    break;
                case "WithdrawnBeforeQualifyingPeriod":
                    x.WithdrawnDate = x.StartDate.AddDays(SFA.DAS.SharedOuterApi.Common.Constants.QualifyingPeriod - 1);
                    break;
            }

            uln = x.Uln;
        });

        // Act
        await _testFixture.CallSubjectUnderTest();// force it to recalculate
        var learningDelivery = _testFixture.Result.FM36Learners.SingleOrDefault(learner => learner.ULN.ToString() == uln).LearningDeliveries.SingleOrDefault();

        // Assert
        learningDelivery.Should().NotBeNull();
        learningDelivery.LearningDeliveryValues.FundStart.Should().Be(expectedFundingStart);

    }
}