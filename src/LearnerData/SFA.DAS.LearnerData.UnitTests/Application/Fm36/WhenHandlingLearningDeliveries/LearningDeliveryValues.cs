using SFA.DAS.LearnerData.Application.Fm36.Common;
using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.LearnerData.UnitTests.Application.Fm36.TestHelpers;
using System.Diagnostics.Eventing.Reader;
using static SFA.DAS.LearnerData.Application.Fm36.Common.EarningsFM36Constants;

namespace SFA.DAS.LearnerData.UnitTests.Application.Fm36.WhenHandlingLearningDeliveries;

public class LearningDeliveryValues
{
    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_HardCodedValues_AreCorrect(TestScenario scenario)
    {
        // Arrange / Act
        var testFixture = await CallFm36Handler(scenario);
        var learningDelivery = testFixture.GetLearningDelivery(scenario);

        // Assert
        learningDelivery!.LearningDeliveryValues.LearnDelApplicProv1618FrameworkUplift.Should().Be(0);
        learningDelivery.LearningDeliveryValues.ActualNumInstalm.Should().BeNull();
        learningDelivery.LearningDeliveryValues.ApplicCompDate.Should().Be(new DateTime(9999, 9, 9));
        learningDelivery.LearningDeliveryValues.CombinedAdjProp.Should().Be(1);
        learningDelivery.LearningDeliveryValues.Completed.Should().BeFalse();
        learningDelivery.LearningDeliveryValues.LDApplic1618FrameworkUpliftTotalActEarnings.Should().Be(0);
        learningDelivery.LearningDeliveryValues.LearnDelApplicDisadvAmount.Should().Be(0);
        learningDelivery.LearningDeliveryValues.LearnDelApplicProv1618FrameworkUplift.Should().Be(0);
        learningDelivery.LearningDeliveryValues.LearnDelDisadAmount.Should().Be(0);
        learningDelivery.LearningDeliveryValues.LearnDelEligDisadvPayment.Should().BeFalse();
        learningDelivery.LearningDeliveryValues.LearnDelEmpIdFirstAdditionalPaymentThreshold.Should().BeNull();
        learningDelivery.LearningDeliveryValues.LearnDelEmpIdSecondAdditionalPaymentThreshold.Should().BeNull();
        learningDelivery.LearningDeliveryValues.LearnDelProgEarliestACT2Date.Should().BeNull();
        learningDelivery.LearningDeliveryValues.LearnDelNonLevyProcured.Should().BeFalse();
        learningDelivery.LearningDeliveryValues.MathEngAimValue.Should().Be(0);
        learningDelivery.LearningDeliveryValues.OutstandNumOnProgInstalm.Should().BeNull();
        learningDelivery.LearningDeliveryValues.ProgType.Should().Be(25);
        learningDelivery.LearningDeliveryValues.PwayCode.Should().BeNull();
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
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_ActualDaysIL_IsCorrect(TestScenario scenario)
    {
        // Arrange / Act
        var testFixture = await CallFm36Handler(scenario);
        var learningDelivery = testFixture.GetLearningDelivery(scenario);

        // Assert
        learningDelivery!.LearningDeliveryValues.ActualDaysIL.Should().Be(0);
    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_AdjStartDate_IsCorrect(TestScenario scenario)
    {
        // Arrange / Act
        var testFixture = await CallFm36Handler(scenario);
        var learningDelivery = testFixture.GetLearningDelivery(scenario);
        var learning = testFixture.UnpagedLearningsResponse.Single();

        // Assert
        learningDelivery!.LearningDeliveryValues.AdjStartDate.Should().Be(learning.StartDate);
    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_AgeAtProgStart_IsCorrect(TestScenario scenario)
    {
        // Arrange / Act
        var testFixture = await CallFm36Handler(scenario);
        var learningDelivery = testFixture.GetLearningDelivery(scenario);
        var learning = testFixture.UnpagedLearningsResponse.Single();

        // Assert
        learningDelivery!.LearningDeliveryValues.AgeAtProgStart.Should().Be(learning.AgeAtStartOfApprenticeship);

    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_AppAdjLearnStartDate_IsCorrect(TestScenario scenario)
    {
        // Arrange / Act
        var testFixture = await CallFm36Handler(scenario);
        var learningDelivery = testFixture.GetLearningDelivery(scenario);
        var learning = testFixture.UnpagedLearningsResponse.Single();

        // Assert
        learningDelivery!.LearningDeliveryValues.AppAdjLearnStartDate.Should().Be(learning.StartDate);
    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_AppAdjLearnStartDateMatchPathway_IsCorrect(TestScenario scenario)
    {
        // Arrange / Act
        var testFixture = await CallFm36Handler(scenario);
        var learningDelivery = testFixture.GetLearningDelivery(scenario);
        var learning = testFixture.UnpagedLearningsResponse.Single();

        // Assert
        learningDelivery!.LearningDeliveryValues.AppAdjLearnStartDateMatchPathway.Should().Be(learning.StartDate);

    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_FirstIncentiveThresholdDate_IsCorrect(TestScenario scenario)
    {
        // Arrange / Act
        var testFixture = await CallFm36Handler(scenario);
        var learningDelivery = testFixture.GetLearningDelivery(scenario);
        var learning = testFixture.UnpagedLearningsResponse.Single();
        var earningEpisode = testFixture.GetEarningEpisode();

        var firstAdditionalPaymentDueDate =
            earningEpisode.AdditionalPayments
            .Where(x => AdditionalPaymentsTypes.Incentives.Contains(x.AdditionalPaymentType))
            .OrderBy(x => x.DueDate).First().DueDate;
        var expectedFirstIncentiveThresholdDate =
            firstAdditionalPaymentDueDate >= learning.StartDate &&
            firstAdditionalPaymentDueDate <= learning.PlannedEndDate
                ? firstAdditionalPaymentDueDate
                : (DateTime?)null;

        // Assert
        if(scenario == TestScenario.ApprenticeshipWithEnglish)
        {
            learningDelivery.LearningDeliveryValues.FirstIncentiveThresholdDate.Should().BeNull();
        }
        else
        {
            learningDelivery.LearningDeliveryValues.FirstIncentiveThresholdDate.Should().Be(expectedFirstIncentiveThresholdDate);
        }
    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_LearnAimRef_IsCorrect(TestScenario scenario)
    {
        // Arrange / Act
        var testFixture = await CallFm36Handler(scenario);
        var learningDelivery = testFixture.GetLearningDelivery(scenario);
        string expectedLearnAimRef = string.Empty;

        if (scenario == TestScenario.ApprenticeshipWithEnglish)
        {
            expectedLearnAimRef = testFixture.SldLearnerData.First().Delivery.EnglishAndMaths.First().LearnAimRef;
        }
        else
        {
            expectedLearnAimRef = testFixture.SldLearnerData.First().Delivery.OnProgramme.First().LearnAimRef;
        }       

        // Assert
        learningDelivery.LearningDeliveryValues.LearnAimRef.Should().Be(expectedLearnAimRef);
    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_LearnStartDate_IsCorrect(TestScenario scenario)
    {
        // Arrange / Act
        var testFixture = await CallFm36Handler(scenario);
        var learningDelivery = testFixture.GetLearningDelivery(scenario);
        var learning = testFixture.UnpagedLearningsResponse.Single();

        DateTime expectedStartDate = DateTime.MinValue;
        if (scenario == TestScenario.ApprenticeshipWithEnglish)
        {
            expectedStartDate = testFixture.SldLearnerData.First().Delivery.EnglishAndMaths.First().StartDate;
        }
        else
        {
            expectedStartDate = learning.StartDate;
        }

        // Assert
        learningDelivery.LearningDeliveryValues.LearnStartDate.Should().Be(expectedStartDate);

    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_LearnDel1618AtStart_IsCorrect(TestScenario scenario)
    {
        // Arrange / Act
        var testFixture = await CallFm36Handler(scenario);
        var learningDelivery = testFixture.GetLearningDelivery(scenario);
        var earningEpisode = testFixture.GetEarningEpisode();

        // Assert
        learningDelivery.LearningDeliveryValues.LearnDel1618AtStart
            .Should()
            .Be(earningEpisode.AdditionalPayments.Any(x =>
                x.AdditionalPaymentType
                    is EarningsFM36Constants.AdditionalPaymentsTypes.EmployerIncentive
                    or EarningsFM36Constants.AdditionalPaymentsTypes.ProviderIncentive));
    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_LearnDelAppAccDaysIL_IsCorrect(TestScenario scenario)
    {
        // Arrange / Act
        var testFixture = await CallFm36Handler(scenario);
        var learningDelivery = testFixture.GetLearningDelivery(scenario);
        var learning = testFixture.UnpagedLearningsResponse.Single();

        var expectedPriceEpisodeEndDate = learning.PlannedEndDate < testFixture.CollectionCalendarResponse.EndDate ? learning.PlannedEndDate : testFixture.CollectionCalendarResponse.EndDate;

        // Assert
        if (scenario == TestScenario.ApprenticeshipWithEnglish)
        {
            learningDelivery.LearningDeliveryValues.LearnDelAppAccDaysIL.Should().BeNull();
        }
        else
        {
            learningDelivery.LearningDeliveryValues.LearnDelAppAccDaysIL.Should().Be(1 + (expectedPriceEpisodeEndDate - learning.StartDate).Days);
        }

    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_LearnDelApplicEmp1618Incentive_IsCorrect(TestScenario scenario)
    {
        // Arrange / Act
        var testFixture = await CallFm36Handler(scenario);
        var learningDelivery = testFixture.GetLearningDelivery(scenario);
        var earningEpisode = testFixture.GetEarningEpisode();

        // Assert
        if (scenario == TestScenario.ApprenticeshipWithEnglish)
        {
            learningDelivery.LearningDeliveryValues.LearnDelApplicEmp1618Incentive.Should().BeNull();
        }
        else
        {
            learningDelivery.LearningDeliveryValues.LearnDelApplicEmp1618Incentive.Should().Be(earningEpisode.AdditionalPayments.Where(x => x.AdditionalPaymentType == "EmployerIncentive").Sum(x => x.Amount));
        }
    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_LearnDelApplicProv1618Incentive_IsCorrect(TestScenario scenario)
    {
        // Arrange / Act
        var testFixture = await CallFm36Handler(scenario);
        var learningDelivery = testFixture.GetLearningDelivery(scenario);
        var earningEpisode = testFixture.GetEarningEpisode();

        // Assert
        if (scenario == TestScenario.ApprenticeshipWithEnglish)
        {
            learningDelivery.LearningDeliveryValues.LearnDelApplicProv1618Incentive.Should().BeNull();
        }
        else
        {
            learningDelivery.LearningDeliveryValues.LearnDelApplicProv1618Incentive.Should().Be(earningEpisode.AdditionalPayments.Where(x => x.AdditionalPaymentType == "ProviderIncentive").Sum(x => x.Amount));
        }
    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_LearnDelAppPrevAccDaysIL_IsCorrect(TestScenario scenario)
    {
        // Arrange / Act
        var testFixture = await CallFm36Handler(scenario);
        var learningDelivery = testFixture.GetLearningDelivery(scenario);
        var learning = testFixture.UnpagedLearningsResponse.Single();

        // Assert
        if (scenario == TestScenario.ApprenticeshipWithEnglish)
        {
            learningDelivery.LearningDeliveryValues.LearnDelAppPrevAccDaysIL.Should().BeNull();
        }
        else
        {
            //This is is simplified and will fail if there are multiple learning deliveries with different LearnerAimRefs
            //It also will not work in the case of Paused, Withdrawn or Completed learnings
            var effEndDate = learning.PlannedEndDate < testFixture.CollectionCalendarResponse.EndDate
                ? learning.PlannedEndDate
                : testFixture.CollectionCalendarResponse.EndDate;

            var learnDelAppPrevAccDaysIL = learning.StartDate.GetNumberOfDaysUntil(effEndDate);

            learningDelivery.LearningDeliveryValues.LearnDelAppPrevAccDaysIL.Should().Be(learnDelAppPrevAccDaysIL);
        }

    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_LearnDelHistDaysThisApp_IsCorrect(TestScenario scenario)
    {
        // Arrange / Act
        var testFixture = await CallFm36Handler(scenario);
        var learningDelivery = testFixture.GetLearningDelivery(scenario);
        var firstSldOnProg = testFixture.SldLearnerData.First().Delivery.OnProgramme.First();

        // note this is a simplified equation and will fail if the value is negative as the actual calc will return 0
        var learnDelHistDaysThisApp = (testFixture.CollectionCalendarResponse.StartDate - firstSldOnProg.StartDate).Days;

        // Assert
        if (scenario == TestScenario.ApprenticeshipWithEnglish)
        {
            learningDelivery.LearningDeliveryValues.LearnDelHistDaysThisApp.Should().BeNull();
        }
        else
        {
            learningDelivery.LearningDeliveryValues.LearnDelHistDaysThisApp.Should().Be(learnDelHistDaysThisApp);
        }
    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_LearnDelHistProgEarnings_IsCorrect(TestScenario scenario)
    {
        // Arrange / Act
        var testFixture = await CallFm36Handler(scenario);
        var learningDelivery = testFixture.GetLearningDelivery(scenario);
        var earningEpisode = testFixture.GetEarningEpisode();

        // Assert
        if (scenario == TestScenario.ApprenticeshipWithEnglish)
        {
            learningDelivery.LearningDeliveryValues.LearnDelHistProgEarnings.Should().BeNull();
        }
        else
        {
            var sum = earningEpisode.Instalments.Sum(i => i.Amount);
            learningDelivery.LearningDeliveryValues.LearnDelHistProgEarnings.Should().Be(sum);
        }
    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_LearnDelInitialFundLineType_IsCorrect(TestScenario scenario)
    {
        // Arrange / Act
        var testFixture = await CallFm36Handler(scenario);
        var learningDelivery = testFixture.GetLearningDelivery(scenario);
        var earningApprenticeship = testFixture.EarningsResponse.Apprenticeships.First();

        // Assert
        learningDelivery.LearningDeliveryValues.LearnDelInitialFundLineType.Should().Be(earningApprenticeship.FundingLineType);
    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_LearnDelMathEng_IsCorrect(TestScenario scenario)
    {
        // Arrange / Act
        var testFixture = await CallFm36Handler(scenario);
        var learningDelivery = testFixture.GetLearningDelivery(scenario);

        // Assert
        if(scenario == TestScenario.ApprenticeshipWithEnglish)
        {
            learningDelivery.LearningDeliveryValues.LearnDelMathEng.Should().BeTrue();
        }
        else
        {
            learningDelivery.LearningDeliveryValues.LearnDelMathEng.Should().BeFalse();
        }
    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_PlannedNumOnProgInstalm_IsCorrect(TestScenario scenario)
    {
        // Arrange / Act
        var testFixture = await CallFm36Handler(scenario);
        var learningDelivery = testFixture.GetLearningDelivery(scenario);
        var learning = testFixture.UnpagedLearningsResponse.Single();

        // Assert
        if (scenario == TestScenario.ApprenticeshipWithEnglish)
        {
            learningDelivery.LearningDeliveryValues.PlannedNumOnProgInstalm.Should().BeNull();
        }
        else
        {
            learningDelivery.LearningDeliveryValues.PlannedNumOnProgInstalm.Should().Be(InstalmentHelper.GetNumberOfInstalmentsBetweenDates(learning.StartDate, learning.PlannedEndDate));
        }
    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_PlannedTotalDaysIL_IsCorrect(TestScenario scenario)
    {
        // Arrange / Act
        var testFixture = await CallFm36Handler(scenario);
        var learningDelivery = testFixture.GetLearningDelivery(scenario);
        var learning = testFixture.UnpagedLearningsResponse.Single();
        var expectedPlannedTotalDays = 1 + (learning.PlannedEndDate - learning.StartDate).Days;

        // Assert
        if (scenario == TestScenario.ApprenticeshipWithEnglish)
        {
            learningDelivery.LearningDeliveryValues.PlannedTotalDaysIL.Should().BeNull();
        }
        else
        {
            learningDelivery.LearningDeliveryValues.PlannedTotalDaysIL.Should().Be(expectedPlannedTotalDays);
        }
    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_SecondIncentiveThresholdDate_IsCorrect(TestScenario scenario)
    {
        // Arrange / Act
        var testFixture = await CallFm36Handler(scenario);
        var learningDelivery = testFixture.GetLearningDelivery(scenario);
        var learning = testFixture.UnpagedLearningsResponse.Single();
        var earningEpisode = testFixture.GetEarningEpisode();

        var secondAdditionalPaymentDueDate =
            earningEpisode.AdditionalPayments
                .Where(x => AdditionalPaymentsTypes.Incentives.Contains(x.AdditionalPaymentType))
                .DistinctBy(x => x.DueDate)
                .OrderBy(x => x.DueDate)
                .Skip(1)
                .FirstOrDefault()?
                .DueDate;
        var expectedSecondIncentiveThresholdDate =
            secondAdditionalPaymentDueDate >= learning.StartDate &&
            secondAdditionalPaymentDueDate <= learning.PlannedEndDate
                ? secondAdditionalPaymentDueDate
                : (DateTime?)null;

        // Assert
        learningDelivery.LearningDeliveryValues.SecondIncentiveThresholdDate.Should().Be(expectedSecondIncentiveThresholdDate);
    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_StdCode_IsCorrect(TestScenario scenario)
    {
        // Arrange / Act
        var testFixture = await CallFm36Handler(scenario);
        var learningDelivery = testFixture.GetLearningDelivery(scenario);
        var learning = testFixture.UnpagedLearningsResponse.Single();

        // Assert
        learningDelivery.LearningDeliveryValues.StdCode.Should().Be(int.Parse(learning.Episodes.Single().TrainingCode));
    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_ThresholdDays_IsCorrect(TestScenario scenario)
    {
        // Arrange / Act
        var testFixture = await CallFm36Handler(scenario);
        var learningDelivery = testFixture.GetLearningDelivery(scenario);

        // Assert
        learningDelivery.LearningDeliveryValues.ThresholdDays.Should().Be(42);
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

    private async Task<GetFm36QueryTestFixture> CallFm36Handler(TestScenario scenario)
    {
        var testFixture = new GetFm36QueryTestFixture(scenario);
        await testFixture.CallSubjectUnderTest();

        return testFixture;
    }

}
