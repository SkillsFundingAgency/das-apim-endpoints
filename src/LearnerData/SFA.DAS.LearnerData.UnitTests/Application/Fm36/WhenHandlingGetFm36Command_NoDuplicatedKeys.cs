using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LearnerData.UnitTests.Application.Fm36.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.LearnerData.UnitTests.Application.Fm36;

// This makes sure that properties like LearningDeliveryPeriodisedValues which are an array of objects with a key
// do not have duplicated keys.
public class WhenHandlingGetFm36Command_NoDuplicatedKeys
{
#pragma warning disable CS8618 // initialised in setup
    private GetFm36CommandTestFixture _testFixture;
#pragma warning restore CS8618 // initialised in setup

    [SetUp]
    public async Task SetUp()
    {
        // Arrange
        _testFixture = new GetFm36CommandTestFixture(TestScenario.AllData);

        // Act
        await _testFixture.CallSubjectUnderTest();
    }

    [Test]
    public void ThenNoDuplicatedKeys()
    {
        // Assert
        _testFixture.Result.Should().NotBeNull();

        foreach (var learner in _testFixture.Result.FM36Learners)
        {
            foreach (var learningDelivery in learner.LearningDeliveries)
            {
                AssertLearningDeliveryPeriodisedValues(learningDelivery.LearningDeliveryPeriodisedValues);
                AssertLearningDeliveryPeriodisedTextValues(learningDelivery.LearningDeliveryPeriodisedTextValues); 
            }
            foreach (var priceEpisode in learner.PriceEpisodes)
            {
                AssertPriceEpisodePeriodisedValues(priceEpisode.PriceEpisodePeriodisedValues);
            }
        }
    }

    private void AssertLearningDeliveryPeriodisedValues(List<LearningDeliveryPeriodisedValues> LearningDeliveryPeriodisedValues)
    {
        var uniqueKeys = new HashSet<string>();
        foreach (var periodisedValue in LearningDeliveryPeriodisedValues)
        {
            if (!uniqueKeys.Add(periodisedValue.AttributeName))
            {
                Assert.Fail($"Duplicated key found: {periodisedValue.AttributeName}");
            }
        }
    }

    private void AssertLearningDeliveryPeriodisedTextValues(List<LearningDeliveryPeriodisedTextValues> LearningDeliveryPeriodisedTextValues)
    {
        var uniqueKeys = new HashSet<string>();
        foreach (var periodisedTextValue in LearningDeliveryPeriodisedTextValues)
        {
            if (!uniqueKeys.Add(periodisedTextValue.AttributeName))
            {
                Assert.Fail($"Duplicated key found: {periodisedTextValue.AttributeName}");
            }
        }
    }

    private void AssertPriceEpisodePeriodisedValues(List<PriceEpisodePeriodisedValues> PriceEpisodePeriodisedValues)
    {
        var uniqueKeys = new HashSet<string>();
        foreach (var periodisedValue in PriceEpisodePeriodisedValues)
        {
            if (!uniqueKeys.Add(periodisedValue.AttributeName))
            {
                Assert.Fail($"Duplicated key found: {periodisedValue.AttributeName}");
            }
        }
    }
}