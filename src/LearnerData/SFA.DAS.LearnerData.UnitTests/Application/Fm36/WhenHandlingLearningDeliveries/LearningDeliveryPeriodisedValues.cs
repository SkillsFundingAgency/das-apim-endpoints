using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using NUnit.Framework.Internal;
using SFA.DAS.LearnerData.Responses.EarningsInner;
using SFA.DAS.LearnerData.UnitTests.Application.Fm36.TestHelpers;
using static SFA.DAS.LearnerData.Application.Fm36.Common.EarningsFM36Constants;

namespace SFA.DAS.LearnerData.UnitTests.Application.Fm36.WhenHandlingLearningDeliveries;

public class LearningDeliveryPeriodisedValues
{
    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_ReturnsDefaultedValues(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        testFixture.Result.Should().NotBeNull();

        var apprenticeship = testFixture.UnpagedLearningsResponse.Single();

        var learningDelivery = testFixture.GetLearningDelivery(scenario);

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
            .Contain(x => x.AttributeName == "ProgrammeAimBalPayment" && x.AllValuesAreSetToZero());
        learningDelivery.LearningDeliveryPeriodisedValues.Should()
            .Contain(x => x.AttributeName == "ProgrammeAimCompletionPayment" && x.AllValuesAreSetToZero());
    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_ReturnsCorrectEnglishAndMathsValues(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        var learningDelivery = testFixture.GetLearningDelivery(scenario);

        if(scenario == TestScenario.ApprenticeshipWithEnglish)
        {
            var academicYearInstalments = GetAcademicYearInstalments(testFixture, scenario);

            var result = learningDelivery.LearningDeliveryPeriodisedValues.SingleOrDefault(x => x.AttributeName == "MathEngOnProgPayment");
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
        else
        {
            learningDelivery.LearningDeliveryPeriodisedValues.Should()
                .Contain(x => x.AttributeName == "MathEngBalPayment" && x.AllValuesAreSetToZero());
            learningDelivery.LearningDeliveryPeriodisedValues.Should()
                .Contain(x => x.AttributeName == "MathEngOnProgPayment" && x.AllValuesAreSetToZero());
        }

    }

    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_ReturnsInstPerPeriodValues(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        testFixture.Result.Should().NotBeNull();

        var academicYearInstalments = GetAcademicYearInstalments(testFixture, scenario);

        var learningDelivery = testFixture.GetLearningDelivery(scenario);
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
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_ReturnsProgrammeAimOnProgPaymentValues(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();


        // Assert
        testFixture.Result.Should().NotBeNull();

        var apprenticeship = testFixture.UnpagedLearningsResponse.Single();

        var academicYearInstalments = GetAcademicYearInstalments(testFixture, scenario);

        var learningDelivery = testFixture.GetLearningDelivery(scenario);
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
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_ReturnsProgrammeAimProgFundIndMaxEmpContValues(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();


        // Assert
        testFixture.Result.Should().NotBeNull();

        var apprenticeship = testFixture.UnpagedLearningsResponse.Single();

        var academicYearInstalments = GetAcademicYearInstalments(testFixture, scenario);

        var learningDelivery = testFixture.GetLearningDelivery(scenario);
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
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_ReturnsProgrammeAimProgFundIndMinCoInvestValues(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();


        // Assert
        testFixture.Result.Should().NotBeNull();

        var apprenticeship = testFixture.UnpagedLearningsResponse.Single();

        var academicYearInstalments = GetAcademicYearInstalments(testFixture, scenario);

        var learningDelivery = testFixture.GetLearningDelivery(scenario);
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
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_ReturnsProgrammeAimTotProgFundValues(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();


        // Assert
        testFixture.Result.Should().NotBeNull();

        var apprenticeship = testFixture.UnpagedLearningsResponse.Single();

        var academicYearInstalments = GetAcademicYearInstalments(testFixture, scenario);

        var learningDelivery = testFixture.GetLearningDelivery(scenario);
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
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_ReturnsLearnDelFirstProv1618PayValues(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        testFixture.Result.Should().NotBeNull();

        var learningDelivery = testFixture.GetLearningDelivery(scenario);
        learningDelivery.LearningDeliveryPeriodisedValues.Should().NotBeNull();
        var result = learningDelivery.LearningDeliveryPeriodisedValues.SingleOrDefault(x => x.AttributeName == "LearnDelFirstProv1618Pay");
        result.Should().NotBeNull();

        AdditionalPayment expectedIncentive = new AdditionalPayment { Amount = 0, DeliveryPeriod = 0 };

        if (scenario != TestScenario.ApprenticeshipWithEnglish)
        {
            var earningEpisode = testFixture.EarningsResponse.Apprenticeships.First().Episodes.Single();
            var providerIncentives = earningEpisode.AdditionalPayments.Where(x => x.AdditionalPaymentType == "ProviderIncentive").ToList();

            expectedIncentive = providerIncentives.FirstOrDefault();
            if (expectedIncentive == null || expectedIncentive.AcademicYear.ToString() != testFixture.CollectionCalendarResponse.AcademicYear)
            {
                expectedIncentive = new AdditionalPayment { Amount = 0, DeliveryPeriod = 0 };
            }
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
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_ReturnsLearnDelSecondProv1618PayValues(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        testFixture.Result.Should().NotBeNull();

        var learningDelivery = testFixture.GetLearningDelivery(scenario);
        learningDelivery.LearningDeliveryPeriodisedValues.Should().NotBeNull();
        var result = learningDelivery.LearningDeliveryPeriodisedValues.SingleOrDefault(x => x.AttributeName == "LearnDelSecondProv1618Pay");
        result.Should().NotBeNull();

        AdditionalPayment expectedIncentive = new AdditionalPayment { Amount = 0, DeliveryPeriod = 0 };

        if (scenario != TestScenario.ApprenticeshipWithEnglish)
        {
            var earningEpisode = testFixture.GetEarningEpisode();
            var providerIncentives = earningEpisode.AdditionalPayments.Where(x => x.AdditionalPaymentType == "ProviderIncentive").ToList();

            expectedIncentive = providerIncentives.Skip(1).FirstOrDefault();
            if (expectedIncentive == null || expectedIncentive.AcademicYear.ToString() != testFixture.CollectionCalendarResponse.AcademicYear)
            {
                expectedIncentive = new AdditionalPayment { Amount = 0, DeliveryPeriod = 0 };
            }
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
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_ReturnsLearnDelFirstEmp1618PayValues(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        testFixture.Result.Should().NotBeNull();

        var learningDelivery = testFixture.GetLearningDelivery(scenario);
        learningDelivery.LearningDeliveryPeriodisedValues.Should().NotBeNull();
        var result = learningDelivery.LearningDeliveryPeriodisedValues.SingleOrDefault(x => x.AttributeName == "LearnDelFirstEmp1618Pay");
        result.Should().NotBeNull();

        AdditionalPayment expectedIncentive = new AdditionalPayment { Amount = 0, DeliveryPeriod = 0 };

        if (scenario != TestScenario.ApprenticeshipWithEnglish)
        {
            var earningEpisode = testFixture.GetEarningEpisode();
            var employerIncentives = earningEpisode.AdditionalPayments.Where(x => x.AdditionalPaymentType == "EmployerIncentive").ToList();

            expectedIncentive = employerIncentives.FirstOrDefault();
            if (expectedIncentive == null || expectedIncentive.AcademicYear.ToString() != testFixture.CollectionCalendarResponse.AcademicYear)
            {
                expectedIncentive = new AdditionalPayment { Amount = 0, DeliveryPeriod = 0 };
            }
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
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_ReturnsLearnDelSecondEmp1618PayValues(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();


        // Assert
        testFixture.Result.Should().NotBeNull();

        AdditionalPayment expectedIncentive = new AdditionalPayment { Amount = 0, DeliveryPeriod = 0 };

        if(scenario != TestScenario.ApprenticeshipWithEnglish)
        {
            var earningEpisode = testFixture.GetEarningEpisode();
            var employerIncentives = earningEpisode.AdditionalPayments.Where(x => x.AdditionalPaymentType == "EmployerIncentive").ToList();

            expectedIncentive = employerIncentives.Skip(1).FirstOrDefault();
            if (expectedIncentive == null || expectedIncentive.AcademicYear.ToString() != testFixture.CollectionCalendarResponse.AcademicYear)
            {
                expectedIncentive = new AdditionalPayment { Amount = 0, DeliveryPeriod = 0 };
            }
        }

        var learningDelivery = testFixture.GetLearningDelivery(scenario);
        learningDelivery.LearningDeliveryPeriodisedValues.Should().NotBeNull();
        var result = learningDelivery.LearningDeliveryPeriodisedValues.SingleOrDefault(x => x.AttributeName == "LearnDelSecondEmp1618Pay");
        result.Should().NotBeNull();

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
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_ReturnsLearningSupportValues(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();

        //Assert
        var learningDelivery = testFixture.GetLearningDelivery(scenario);

        List<AdditionalPayment> expectedLearningSupport;

        if (scenario == TestScenario.ApprenticeshipWithEnglish)
        {
            expectedLearningSupport = new List<AdditionalPayment>(); // In this test scenario, there are learning support
                                                                     // payments but they all fall within onprogramme date range
                                                                     // Learning support should only be recorded against English and maths
                                                                     // if the payments fall outside of the onprogramme date range.
                                                                     // this is covered in a seperate test scenario
        }
        else
        {
            expectedLearningSupport = testFixture.EarningsResponse.Apprenticeships.First()
                .Episodes.First()
                .AdditionalPayments.Where(x =>
                    x.AdditionalPaymentType == AdditionalPaymentsTypes.LearningSupport &&
                    x.AcademicYear == short.Parse(testFixture.CollectionCalendarResponse.AcademicYear))
                .ToList();
        }


        AssertLearningSupport(learningDelivery, expectedLearningSupport);
    }

    [Test]
    public async Task Then_ReturnsLearningSupportValues_WhenComplexScenario()
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(TestScenario.LearningSupportComplexScenario);

        // Act
        await testFixture.CallSubjectUnderTest();

        //Assert
        var apprenticeship = testFixture.UnpagedLearningsResponse.Single();
        var onPrgrammeLearningDelivery = testFixture.GetLearningDeliveryByAimSequenceNumber(1);
        var englishLearningDelivery = testFixture.GetLearningDeliveryByAimSequenceNumber(2);
        var mathsLearningDelivery = testFixture.GetLearningDeliveryByAimSequenceNumber(3);

        List<AdditionalPayment> expectedLearningSupport;

        AssertLearningSupport(onPrgrammeLearningDelivery, new List<AdditionalPayment>
        {
            new AdditionalPayment {DeliveryPeriod = 3, Amount = 150 },
            new AdditionalPayment {DeliveryPeriod = 4, Amount = 150 },
            new AdditionalPayment {DeliveryPeriod = 5, Amount = 150 },
        });

        AssertLearningSupport(englishLearningDelivery, new List<AdditionalPayment>());

        AssertLearningSupport(mathsLearningDelivery, new List<AdditionalPayment>
        {
            new AdditionalPayment {DeliveryPeriod = 6, Amount = 150 },
            new AdditionalPayment {DeliveryPeriod = 7, Amount = 150 },
            new AdditionalPayment {DeliveryPeriod = 8, Amount = 150 },
        });
    }

    private void AssertLearningSupport(
        ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output.LearningDelivery learningDelivery, List<AdditionalPayment> expectedLearningSupport)
    {

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

    private static List<IInstalment> GetAcademicYearInstalments(GetFm36QueryTestFixture testFixture, TestScenario scenario)
    {
        var earningEpisode = testFixture.GetEarningEpisode();
        if (scenario == TestScenario.ApprenticeshipWithEnglish)
        {
            return earningEpisode.EnglishAndMaths.Single().Instalments.Where(x => x.AcademicYear == short.Parse(testFixture.CollectionCalendarResponse.AcademicYear)).ToList<IInstalment>();
        }
        else
        {
            return earningEpisode.Instalments.Where(x => x.AcademicYear == short.Parse(testFixture.CollectionCalendarResponse.AcademicYear)).ToList<IInstalment>();
        }
    }
    
}