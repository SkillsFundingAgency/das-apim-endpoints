using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LearnerData.Application.Fm36.Common;
using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.LearnerData.UnitTests.Application.Fm36.TestHelpers;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using static SFA.DAS.LearnerData.Application.Fm36.Common.EarningsFM36Constants;

namespace SFA.DAS.LearnerData.UnitTests.Application.Fm36;

#pragma warning disable CS8602 // Defrerred nullability warnings for test cases
public class WhenHandlingGetFm36Query_LearningDeliveries
{
    [Test]
    public async Task ThenALearningDeliveryIsCreatedForEachApprenticeship()
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(TestScenario.AllData);

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        testFixture.Result.Should().NotBeNull();

        testFixture.Result.Items.Count.Should().Be(testFixture.UnpagedLearningsResponse.Count);
        testFixture.Result.Items.SelectMany(learner => learner.LearningDeliveries).Count().Should().Be(testFixture.UnpagedLearningsResponse.Count);
    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    public async Task ThenReturnsLearningDeliveryValuesForEachApprenticeship(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        testFixture.Result.Should().NotBeNull();

        var apprenticeship = testFixture.UnpagedLearningsResponse.Single();

        var expectedPriceEpisodeStartDate = apprenticeship.StartDate > testFixture.CollectionCalendarResponse.StartDate ? apprenticeship.StartDate : testFixture.CollectionCalendarResponse.StartDate;
        var expectedPriceEpisodeEndDate = apprenticeship.PlannedEndDate < testFixture.CollectionCalendarResponse.EndDate ? apprenticeship.PlannedEndDate : testFixture.CollectionCalendarResponse.EndDate;
        var earningApprenticeship = testFixture.EarningsResponse.Apprenticeships.First();
        var earningEpisode = earningApprenticeship.Episodes.Single();
        var firstSldOnProg = testFixture.SldLearnerData.First().Delivery.OnProgramme.First();

        //This is is simplified and will fail if there are multiple learning deliveries with different LearnerAimRefs
        //It also will not work in the case of Paused, Withdrawn or Completed learnings
        var effEndDate = apprenticeship.PlannedEndDate < testFixture.CollectionCalendarResponse.EndDate
            ? apprenticeship.PlannedEndDate
            : testFixture.CollectionCalendarResponse.EndDate;

        var learnDelAppPrevAccDaysIL = apprenticeship.StartDate.GetNumberOfDaysUntil(effEndDate);

        var learningDelivery = testFixture.Result.Items.SingleOrDefault(learner => learner.ULN.ToString() == apprenticeship.Uln).LearningDeliveries.SingleOrDefault();

        // note this is a simplified equation and will fail if the value is negative as the actual calc will return 0
        var learnDelHistDaysThisApp = (testFixture.CollectionCalendarResponse.StartDate - firstSldOnProg.StartDate).Days;

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
            earningEpisode.AdditionalPayments
                .Where(x => AdditionalPaymentsTypes.Incentives.Contains(x.AdditionalPaymentType))
                .OrderBy(x => x.DueDate).First().DueDate;
        var expectedFirstIncentiveThresholdDate =
            firstAdditionalPaymentDueDate >= apprenticeship.StartDate &&
            firstAdditionalPaymentDueDate <= apprenticeship.PlannedEndDate
                ? firstAdditionalPaymentDueDate
                : (DateTime?)null;
        learningDelivery.LearningDeliveryValues.FirstIncentiveThresholdDate.Should().Be(expectedFirstIncentiveThresholdDate);
        learningDelivery.LearningDeliveryValues.LDApplic1618FrameworkUpliftTotalActEarnings.Should().Be(0);
        learningDelivery.LearningDeliveryValues.LearnAimRef.Should().Be(firstSldOnProg.LearnAimRef);
        learningDelivery.LearningDeliveryValues.LearnStartDate.Should().Be(apprenticeship.StartDate);
        learningDelivery.LearningDeliveryValues.LearnDel1618AtStart
            .Should()
            .Be(earningEpisode.AdditionalPayments.Any(x =>
                x.AdditionalPaymentType
                    is EarningsFM36Constants.AdditionalPaymentsTypes.EmployerIncentive
                    or EarningsFM36Constants.AdditionalPaymentsTypes.ProviderIncentive));
        learningDelivery.LearningDeliveryValues.LearnDelAppAccDaysIL.Should().Be(1 + (expectedPriceEpisodeEndDate - apprenticeship.StartDate).Days);
        learningDelivery.LearningDeliveryValues.LearnDelApplicDisadvAmount.Should().Be(0);
        learningDelivery.LearningDeliveryValues.LearnDelApplicEmp1618Incentive.Should().Be(earningEpisode.AdditionalPayments.Where(x => x.AdditionalPaymentType == "EmployerIncentive").Sum(x => x.Amount));
        learningDelivery.LearningDeliveryValues.LearnDelApplicProv1618FrameworkUplift.Should().Be(0);
        learningDelivery.LearningDeliveryValues.LearnDelApplicProv1618Incentive.Should().Be(earningEpisode.AdditionalPayments.Where(x => x.AdditionalPaymentType == "ProviderIncentive").Sum(x => x.Amount));
        learningDelivery.LearningDeliveryValues.LearnDelAppPrevAccDaysIL.Should().Be(learnDelAppPrevAccDaysIL);
        learningDelivery.LearningDeliveryValues.LearnDelDisadAmount.Should().Be(0);
        learningDelivery.LearningDeliveryValues.LearnDelEligDisadvPayment.Should().BeFalse();
        learningDelivery.LearningDeliveryValues.LearnDelEmpIdFirstAdditionalPaymentThreshold.Should().BeNull();
        learningDelivery.LearningDeliveryValues.LearnDelEmpIdSecondAdditionalPaymentThreshold.Should().BeNull();
        learningDelivery.LearningDeliveryValues.LearnDelHistDaysThisApp.Should().Be(learnDelHistDaysThisApp);
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
                .Where(x => AdditionalPaymentsTypes.Incentives.Contains(x.AdditionalPaymentType))
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

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    public async Task ThenReturnsDefaultedLearningDeliveryPeriodisedValuesForEachApprenticeship(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        testFixture.Result.Should().NotBeNull();

        var apprenticeship = testFixture.UnpagedLearningsResponse.Single();

        var learningDelivery = testFixture.Result.Items.SingleOrDefault(learner => learner.ULN.ToString() == apprenticeship.Uln).LearningDeliveries.SingleOrDefault();
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
            .Contain(x => x.AttributeName == "MathEngBalPayment" && x.AllValuesAreSetToZero());
        learningDelivery.LearningDeliveryPeriodisedValues.Should()
            .Contain(x => x.AttributeName == "MathEngOnProgPayment" && x.AllValuesAreSetToZero());
        learningDelivery.LearningDeliveryPeriodisedValues.Should()
            .Contain(x => x.AttributeName == "ProgrammeAimBalPayment" && x.AllValuesAreSetToZero());
        learningDelivery.LearningDeliveryPeriodisedValues.Should()
            .Contain(x => x.AttributeName == "ProgrammeAimCompletionPayment" && x.AllValuesAreSetToZero());
    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    public async Task ThenReturnsLearningSupportValues(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();

        //Assert
        var apprenticeship = testFixture.UnpagedLearningsResponse.Single();
        var learningDelivery = testFixture.Result.Items.SingleOrDefault(learner => learner.ULN.ToString() == apprenticeship.Uln).LearningDeliveries.SingleOrDefault();

        var expectedLearningSupport = testFixture.EarningsResponse.Apprenticeships.First()
            .Episodes.First()
            .AdditionalPayments.Where(x =>
                x.AdditionalPaymentType == AdditionalPaymentsTypes.LearningSupport &&
                x.AcademicYear == short.Parse(testFixture.CollectionCalendarResponse.AcademicYear))
            .ToList();

        var valueResult = learningDelivery.LearningDeliveryPeriodisedValues.Single(x => x.AttributeName == PeriodisedAttributes.LearnSuppFundCash);
        valueResult.Period1.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 1)?.Amount ?? 0);
        valueResult.Period2.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 2)?.Amount ?? 0);
        valueResult.Period3.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 3)?.Amount ?? 0);
        valueResult.Period4.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 4)?.Amount ?? 0);
        valueResult.Period5.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 5)?.Amount ?? 0);
        valueResult.Period6.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 6)?.Amount ?? 0);
        valueResult.Period7.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 7)?.Amount ?? 0);
        valueResult.Period8.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 8)?.Amount ?? 0);
        valueResult.Period9.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 9)?.Amount ?? 0);
        valueResult.Period10.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 10)?.Amount ?? 0);
        valueResult.Period11.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 11)?.Amount ?? 0);
        valueResult.Period12.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 12)?.Amount ?? 0);

        var indicatorResult = learningDelivery.LearningDeliveryPeriodisedValues.Single(x => x.AttributeName == PeriodisedAttributes.LearnSuppFund);
        indicatorResult.Period1.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 1)?.Amount > 0 ? 1 : 0);
        indicatorResult.Period2.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 2)?.Amount > 0 ? 1 : 0);
        indicatorResult.Period3.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 3)?.Amount > 0 ? 1 : 0);
        indicatorResult.Period4.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 4)?.Amount > 0 ? 1 : 0);
        indicatorResult.Period5.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 5)?.Amount > 0 ? 1 : 0);
        indicatorResult.Period6.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 6)?.Amount > 0 ? 1 : 0);
        indicatorResult.Period7.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 7)?.Amount > 0 ? 1 : 0);
        indicatorResult.Period8.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 8)?.Amount > 0 ? 1 : 0);
        indicatorResult.Period9.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 9)?.Amount > 0 ? 1 : 0);
        indicatorResult.Period10.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 10)?.Amount > 0 ? 1 : 0);
        indicatorResult.Period11.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 11)?.Amount > 0 ? 1 : 0);
        indicatorResult.Period12.Should().Be(expectedLearningSupport.SingleOrDefault(x => x.DeliveryPeriod == 12)?.Amount > 0 ? 1 : 0);
    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    public async Task ThenReturnsInstPerPeriodValuesForEachApprenticeship(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        testFixture.Result.Should().NotBeNull();

        var apprenticeship = testFixture.UnpagedLearningsResponse.Single();

        var earningEpisode = testFixture.EarningsResponse.Apprenticeships.First().Episodes.Single();
        var academicYearInstalments = earningEpisode.Instalments.Where(x => x.AcademicYear == short.Parse(testFixture.CollectionCalendarResponse.AcademicYear)).ToList();

        var learningDelivery = testFixture.Result.Items.SingleOrDefault(learner => learner.ULN.ToString() == apprenticeship.Uln).LearningDeliveries.SingleOrDefault();
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

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    public async Task ThenReturnsProgrammeAimOnProgPaymentValuesForEachApprenticeship(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();


        // Assert
        testFixture.Result.Should().NotBeNull();

        var apprenticeship = testFixture.UnpagedLearningsResponse.Single();

        var earningEpisode = testFixture.EarningsResponse.Apprenticeships.First().Episodes.Single();
        var academicYearInstalments = earningEpisode.Instalments.Where(x => x.AcademicYear == short.Parse(testFixture.CollectionCalendarResponse.AcademicYear)).ToList();

        var learningDelivery = testFixture.Result.Items.SingleOrDefault(learner => learner.ULN.ToString() == apprenticeship.Uln).LearningDeliveries.SingleOrDefault();
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

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    public async Task ThenReturnsProgrammeAimProgFundIndMaxEmpContValuesForEachApprenticeship(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();


        // Assert
        testFixture.Result.Should().NotBeNull();

        var apprenticeship = testFixture.UnpagedLearningsResponse.Single();

        var earningEpisode = testFixture.EarningsResponse.Apprenticeships.First().Episodes.Single();
        var academicYearInstalments = earningEpisode.Instalments.Where(x => x.AcademicYear == short.Parse(testFixture.CollectionCalendarResponse.AcademicYear)).ToList();

        var learningDelivery = testFixture.Result.Items.SingleOrDefault(learner => learner.ULN.ToString() == apprenticeship.Uln).LearningDeliveries.SingleOrDefault();
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

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    public async Task ThenReturnsProgrammeAimProgFundIndMinCoInvestValuesForEachApprenticeship(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();


        // Assert
        testFixture.Result.Should().NotBeNull();

        var apprenticeship = testFixture.UnpagedLearningsResponse.Single();

        var earningEpisode = testFixture.EarningsResponse.Apprenticeships.First().Episodes.Single();
        var academicYearInstalments = earningEpisode.Instalments.Where(x => x.AcademicYear == short.Parse(testFixture.CollectionCalendarResponse.AcademicYear)).ToList();

        var learningDelivery = testFixture.Result.Items.SingleOrDefault(learner => learner.ULN.ToString() == apprenticeship.Uln).LearningDeliveries.SingleOrDefault();
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

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    public async Task ThenReturnsProgrammeAimTotProgFundValuesForEachApprenticeship(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();


        // Assert
        testFixture.Result.Should().NotBeNull();

        var apprenticeship = testFixture.UnpagedLearningsResponse.Single();

        var earningEpisode = testFixture.EarningsResponse.Apprenticeships.First().Episodes.Single();
        var academicYearInstalments = earningEpisode.Instalments.Where(x => x.AcademicYear == short.Parse(testFixture.CollectionCalendarResponse.AcademicYear)).ToList();

        var learningDelivery = testFixture.Result.Items.SingleOrDefault(learner => learner.ULN.ToString() == apprenticeship.Uln).LearningDeliveries.SingleOrDefault();
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

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    public async Task ThenReturnsLearningDeliveryPeriodisedTextValuesForEachApprenticeship(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();


        // Assert
        testFixture.Result.Should().NotBeNull();

        var apprenticeship = testFixture.UnpagedLearningsResponse.Single();
        var earningsApprenticeship = testFixture.EarningsResponse.Apprenticeships.First();

        var learningDelivery = testFixture.Result.Items.SingleOrDefault(learner => learner.ULN.ToString() == apprenticeship.Uln).LearningDeliveries.SingleOrDefault();
        learningDelivery.Should().NotBeNull();
        learningDelivery.LearningDeliveryPeriodisedTextValues.Should().NotBeNull();
        learningDelivery.LearningDeliveryPeriodisedTextValues.Should().Contain(x =>
            x.AttributeName == "FundLineType" && x.AllValuesAreSetTo(earningsApprenticeship.FundingLineType));
        learningDelivery.LearningDeliveryPeriodisedTextValues.Should().Contain(x =>
            x.AttributeName == "LearnDelContType" && x.AllValuesAreSetTo("Contract for services with the employer"));
    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]

    public async Task ThenReturnsLearningDeliveryPeriodisedLearnDelFirstProv1618PayValuesForEachApprenticeship(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        testFixture.Result.Should().NotBeNull();

        var apprenticeship = testFixture.UnpagedLearningsResponse.Single();

        var earningEpisode = testFixture.EarningsResponse.Apprenticeships.First().Episodes.Single();
        var providerIncentives = earningEpisode.AdditionalPayments.Where(x => x.AdditionalPaymentType == "ProviderIncentive").ToList();

        var learningDelivery = testFixture.Result.Items.SingleOrDefault(learner => learner.ULN.ToString() == apprenticeship.Uln).LearningDeliveries.SingleOrDefault();
        learningDelivery.Should().NotBeNull();
        learningDelivery.LearningDeliveryPeriodisedValues.Should().NotBeNull();
        var result = learningDelivery.LearningDeliveryPeriodisedValues.SingleOrDefault(x => x.AttributeName == "LearnDelFirstProv1618Pay");
        result.Should().NotBeNull();

        var expectedIncentive = providerIncentives.FirstOrDefault();
        if (expectedIncentive == null || expectedIncentive.AcademicYear.ToString() != testFixture.CollectionCalendarResponse.AcademicYear)
        {
            expectedIncentive = new AdditionalPayment { Amount = 0, DeliveryPeriod = 0 };
        }

        result.Period1.Should().Be((expectedIncentive?.DeliveryPeriod == 1 ? expectedIncentive.Amount : 0));
        result.Period2.Should().Be((expectedIncentive?.DeliveryPeriod == 2 ? expectedIncentive.Amount : 0));
        result.Period3.Should().Be((expectedIncentive?.DeliveryPeriod == 3 ? expectedIncentive.Amount : 0));
        result.Period4.Should().Be((expectedIncentive?.DeliveryPeriod == 4 ? expectedIncentive.Amount : 0));
        result.Period5.Should().Be((expectedIncentive?.DeliveryPeriod == 5 ? expectedIncentive.Amount : 0));
        result.Period6.Should().Be((expectedIncentive?.DeliveryPeriod == 6 ? expectedIncentive.Amount : 0));
        result.Period7.Should().Be((expectedIncentive?.DeliveryPeriod == 7 ? expectedIncentive.Amount : 0));
        result.Period8.Should().Be((expectedIncentive?.DeliveryPeriod == 8 ? expectedIncentive.Amount : 0));
        result.Period9.Should().Be((expectedIncentive?.DeliveryPeriod == 9 ? expectedIncentive.Amount : 0));
        result.Period10.Should().Be((expectedIncentive?.DeliveryPeriod == 10 ? expectedIncentive.Amount : 0));
        result.Period11.Should().Be((expectedIncentive?.DeliveryPeriod == 11 ? expectedIncentive.Amount : 0));
        result.Period12.Should().Be((expectedIncentive?.DeliveryPeriod == 12 ? expectedIncentive.Amount : 0));
    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]

    public async Task ThenReturnsLearningDeliveryPeriodisedLearnDelSecondProv1618PayValuesForEachApprenticeship(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        testFixture.Result.Should().NotBeNull();

        var apprenticeship = testFixture.UnpagedLearningsResponse.Single();

        var earningEpisode = testFixture.EarningsResponse.Apprenticeships.First().Episodes.Single();
        var providerIncentives = earningEpisode.AdditionalPayments.Where(x => x.AdditionalPaymentType == "ProviderIncentive").ToList();

        var learningDelivery = testFixture.Result.Items.SingleOrDefault(learner => learner.ULN.ToString() == apprenticeship.Uln).LearningDeliveries.SingleOrDefault();
        learningDelivery.Should().NotBeNull();
        learningDelivery.LearningDeliveryPeriodisedValues.Should().NotBeNull();
        var result = learningDelivery.LearningDeliveryPeriodisedValues.SingleOrDefault(x => x.AttributeName == "LearnDelSecondProv1618Pay");
        result.Should().NotBeNull();

        var expectedIncentive = providerIncentives.Skip(1).FirstOrDefault();
        if (expectedIncentive == null || expectedIncentive.AcademicYear.ToString() != testFixture.CollectionCalendarResponse.AcademicYear)
        {
            expectedIncentive = new AdditionalPayment { Amount = 0, DeliveryPeriod = 0 };
        }

        result.Period1.Should().Be((expectedIncentive?.DeliveryPeriod == 1 ? expectedIncentive.Amount : 0));
        result.Period2.Should().Be((expectedIncentive?.DeliveryPeriod == 2 ? expectedIncentive.Amount : 0));
        result.Period3.Should().Be((expectedIncentive?.DeliveryPeriod == 3 ? expectedIncentive.Amount : 0));
        result.Period4.Should().Be((expectedIncentive?.DeliveryPeriod == 4 ? expectedIncentive.Amount : 0));
        result.Period5.Should().Be((expectedIncentive?.DeliveryPeriod == 5 ? expectedIncentive.Amount : 0));
        result.Period6.Should().Be((expectedIncentive?.DeliveryPeriod == 6 ? expectedIncentive.Amount : 0));
        result.Period7.Should().Be((expectedIncentive?.DeliveryPeriod == 7 ? expectedIncentive.Amount : 0));
        result.Period8.Should().Be((expectedIncentive?.DeliveryPeriod == 8 ? expectedIncentive.Amount : 0));
        result.Period9.Should().Be((expectedIncentive?.DeliveryPeriod == 9 ? expectedIncentive.Amount : 0));
        result.Period10.Should().Be((expectedIncentive?.DeliveryPeriod == 10 ? expectedIncentive.Amount : 0));
        result.Period11.Should().Be((expectedIncentive?.DeliveryPeriod == 11 ? expectedIncentive.Amount : 0));
        result.Period12.Should().Be((expectedIncentive?.DeliveryPeriod == 12 ? expectedIncentive.Amount : 0));
    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]

    public async Task ThenReturnsLearningDeliveryPeriodisedLearnDelFirstEmp1618PayValuesForEachApprenticeship(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        testFixture.Result.Should().NotBeNull();

        var apprenticeship = testFixture.UnpagedLearningsResponse.Single();

        var earningEpisode = testFixture.EarningsResponse.Apprenticeships.First().Episodes.Single();
        var employerIncentives = earningEpisode.AdditionalPayments.Where(x => x.AdditionalPaymentType == "EmployerIncentive").ToList();

        var learningDelivery = testFixture.Result.Items.SingleOrDefault(learner => learner.ULN.ToString() == apprenticeship.Uln).LearningDeliveries.SingleOrDefault();
        learningDelivery.Should().NotBeNull();
        learningDelivery.LearningDeliveryPeriodisedValues.Should().NotBeNull();
        var result = learningDelivery.LearningDeliveryPeriodisedValues.SingleOrDefault(x => x.AttributeName == "LearnDelFirstEmp1618Pay");
        result.Should().NotBeNull();

        var expectedIncentive = employerIncentives.FirstOrDefault();
        if (expectedIncentive == null || expectedIncentive.AcademicYear.ToString() != testFixture.CollectionCalendarResponse.AcademicYear)
        {
            expectedIncentive = new AdditionalPayment { Amount = 0, DeliveryPeriod = 0 };
        }

        result.Period1.Should().Be((expectedIncentive?.DeliveryPeriod == 1 ? expectedIncentive?.Amount : 0));
        result.Period2.Should().Be((expectedIncentive?.DeliveryPeriod == 2 ? expectedIncentive?.Amount : 0));
        result.Period3.Should().Be((expectedIncentive?.DeliveryPeriod == 3 ? expectedIncentive?.Amount : 0));
        result.Period4.Should().Be((expectedIncentive?.DeliveryPeriod == 4 ? expectedIncentive?.Amount : 0));
        result.Period5.Should().Be((expectedIncentive?.DeliveryPeriod == 5 ? expectedIncentive?.Amount : 0));
        result.Period6.Should().Be((expectedIncentive?.DeliveryPeriod == 6 ? expectedIncentive?.Amount : 0));
        result.Period7.Should().Be((expectedIncentive?.DeliveryPeriod == 7 ? expectedIncentive?.Amount : 0));
        result.Period8.Should().Be((expectedIncentive?.DeliveryPeriod == 8 ? expectedIncentive?.Amount : 0));
        result.Period9.Should().Be((expectedIncentive?.DeliveryPeriod == 9 ? expectedIncentive?.Amount : 0));
        result.Period10.Should().Be((expectedIncentive?.DeliveryPeriod == 10 ? expectedIncentive?.Amount : 0));
        result.Period11.Should().Be((expectedIncentive?.DeliveryPeriod == 11 ? expectedIncentive?.Amount : 0));
        result.Period12.Should().Be((expectedIncentive?.DeliveryPeriod == 12 ? expectedIncentive?.Amount : 0));
    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]

    public async Task ThenReturnsLearningDeliveryPeriodisedLearnDelSecondEmp1618PayValuesForEachApprenticeship(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();


        // Assert
        testFixture.Result.Should().NotBeNull();

        var apprenticeship = testFixture.UnpagedLearningsResponse.Single();

        var earningEpisode = testFixture.EarningsResponse.Apprenticeships.First().Episodes.Single();
        var employerIncentives = earningEpisode.AdditionalPayments.Where(x => x.AdditionalPaymentType == "EmployerIncentive").ToList();

        var learningDelivery = testFixture.Result.Items.SingleOrDefault(learner => learner.ULN.ToString() == apprenticeship.Uln).LearningDeliveries.SingleOrDefault();
        learningDelivery.Should().NotBeNull();
        learningDelivery.LearningDeliveryPeriodisedValues.Should().NotBeNull();
        var result = learningDelivery.LearningDeliveryPeriodisedValues.SingleOrDefault(x => x.AttributeName == "LearnDelSecondEmp1618Pay");
        result.Should().NotBeNull();

        var expectedIncentive = employerIncentives.Skip(1).FirstOrDefault();
        if (expectedIncentive == null || expectedIncentive.AcademicYear.ToString() != testFixture.CollectionCalendarResponse.AcademicYear)
        {
            expectedIncentive = new AdditionalPayment { Amount = 0, DeliveryPeriod = 0 };
        }

        result.Period1.Should().Be((expectedIncentive?.DeliveryPeriod == 1 ? expectedIncentive.Amount : 0));
        result.Period2.Should().Be((expectedIncentive?.DeliveryPeriod == 2 ? expectedIncentive.Amount : 0));
        result.Period3.Should().Be((expectedIncentive?.DeliveryPeriod == 3 ? expectedIncentive.Amount : 0));
        result.Period4.Should().Be((expectedIncentive?.DeliveryPeriod == 4 ? expectedIncentive.Amount : 0));
        result.Period5.Should().Be((expectedIncentive?.DeliveryPeriod == 5 ? expectedIncentive.Amount : 0));
        result.Period6.Should().Be((expectedIncentive?.DeliveryPeriod == 6 ? expectedIncentive.Amount : 0));
        result.Period7.Should().Be((expectedIncentive?.DeliveryPeriod == 7 ? expectedIncentive.Amount : 0));
        result.Period8.Should().Be((expectedIncentive?.DeliveryPeriod == 8 ? expectedIncentive.Amount : 0));
        result.Period9.Should().Be((expectedIncentive?.DeliveryPeriod == 9 ? expectedIncentive.Amount : 0));
        result.Period10.Should().Be((expectedIncentive?.DeliveryPeriod == 10 ? expectedIncentive.Amount : 0));
        result.Period11.Should().Be((expectedIncentive?.DeliveryPeriod == 11 ? expectedIncentive.Amount : 0));
        result.Period12.Should().Be((expectedIncentive?.DeliveryPeriod == 12 ? expectedIncentive.Amount : 0));
    }

    [TestCase(WithdrawalDate.None, true)]
    [TestCase(WithdrawalDate.AfterQualifyingPeriod, true)]
    [TestCase(WithdrawalDate.DuringQualifyingPeriod, false)]
    public async Task ThenReturnsCorrectFundStartValueForApprenticeship(WithdrawalDate withdrawalDate, bool expectedFundingStart)
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(TestScenario.SimpleApprenticeship);
        testFixture.UnpagedLearningsResponse.First().SetWithdrawalDate(withdrawalDate);

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        var learningDelivery = testFixture.Result.Items.Single().LearningDeliveries.SingleOrDefault();
        learningDelivery.Should().NotBeNull();
        learningDelivery.LearningDeliveryValues.FundStart.Should().Be(expectedFundingStart);
    }
}
#pragma warning restore CS8602 // Defrerred nullability warnings for test cases