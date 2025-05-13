using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LearnerData.Requests;

namespace SFA.DAS.LearnerData.Api.UnitTests.Controllers;

public class WhenValidatingLearnerData
{
    [Test]
    public void And_when_AreAllValid_Then_No_Errors_returned()
    {
        var learner = CreateValidLearnerDataRequest();
        var validationResults = RunValidation(learner);
        validationResults.Any().Should().BeFalse();
    }

    [Test]
    public void And_when_ULN_IsNotValid_Then_BadRequest_returned()
    {
        var learner = CreateValidLearnerDataRequest();
        learner.ULN = 1234;
        var validationResults = RunValidation(learner);
        validationResults.Any().Should().BeTrue();
        validationResults.First().ErrorMessage.Should().Be($"Learner data contains incorrect ULN {learner.ULN}");
        validationResults.First().MemberNames.Should().Contain("ULN");
    }

    [Test]
    public void And_when_ConsumerReference_Is_Too_Big()
    {
        var learner = CreateValidLearnerDataRequest();
        learner.ConsumerReference = new string('C', 101); 
        var validationResults = RunValidation(learner);
        validationResults.Any().Should().BeTrue();
        validationResults.First().MemberNames.Should().Contain("ConsumerReference");
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void And_Firstname_Is_blank(string? name)
    {
        var learner = CreateValidLearnerDataRequest();
        learner.FirstName = name;
        var validationResults = RunValidation(learner);
        validationResults.Any().Should().BeTrue();
        validationResults.First().MemberNames.Should().Contain("FirstName");
    }


    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void And_Lastname_Is_blank(string? name)
    {
        var learner = CreateValidLearnerDataRequest();
        learner.LastName = name;
        var validationResults = RunValidation(learner);
        validationResults.Any().Should().BeTrue();
        validationResults.First().MemberNames.Should().Contain("LastName");
    }

    [Test]
    public void And_EmailAddress_Is_Invalid()
    {
        var learner = CreateValidLearnerDataRequest();
        learner.Email = "NotValids.com";
        var validationResults = RunValidation(learner);
        validationResults.Any().Should().BeTrue();
        validationResults.First().MemberNames.Should().Contain("Email");
    }

    [Test]
    public void And_EPAOPrice_Is_Negative()
    {
        var learner = CreateValidLearnerDataRequest();
        learner.EpaoPrice = -1;
        var validationResults = RunValidation(learner);
        validationResults.Any().Should().BeTrue();
        validationResults.First().ErrorMessage.Should().Be($"Learner data contains a negative EpaoPrice {learner.EpaoPrice}");
        validationResults.First().MemberNames.Should().Contain("EpaoPrice");
    }

    [Test]
    public void And_TrainingPrice_Is_Negative()
    {
        var learner = CreateValidLearnerDataRequest();
        learner.TrainingPrice = -1;
        var validationResults = RunValidation(learner);
        validationResults.Any().Should().BeTrue();
        validationResults.First().ErrorMessage.Should().Be($"Learner data contains a negative TrainingPrice {learner.TrainingPrice}");
        validationResults.First().MemberNames.Should().Contain("TrainingPrice");
    }

    [Test]
    public void And_PlannedOTJTrainingHours_Is_Negative()
    {
        var learner = CreateValidLearnerDataRequest();
        learner.PlannedOTJTrainingHours = -1;
        var validationResults = RunValidation(learner);
        validationResults.Any().Should().BeTrue();
        validationResults.First().ErrorMessage.Should().Be($"Learner data contains a negative PlannedOTJTrainingHours {learner.PlannedOTJTrainingHours}");
        validationResults.First().MemberNames.Should().Contain("PlannedOTJTrainingHours");
    }

    [Test]
    public void And_StandardCode_Is_Negative()
    {
        var learner = CreateValidLearnerDataRequest();
        learner.StandardCode = -1;
        var validationResults = RunValidation(learner);
        validationResults.Any().Should().BeTrue();
        validationResults.First().ErrorMessage.Should().Be($"Learner data contains a negative StandardCode {learner.StandardCode}");
        validationResults.First().MemberNames.Should().Contain("StandardCode");
    }

    private LearnerDataRequest CreateValidLearnerDataRequest()
    {
        return new LearnerDataRequest
        {
            ULN = 1234567890,
            UKPRN = 12345678,
            FirstName = "First",
            LastName = "Last",
            Email = "Email@abcd.com",
            DoB = new DateTime(2000, 02, 01),
            StartDate = new DateTime(2025, 02, 01),
            PlannedEndDate = new DateTime(2027, 02, 01),
            PercentageLearningToBeDelivered = null,
            EpaoPrice = 400,
            TrainingPrice = 3200,
            AgreementId = "ABCD",
            IsFlexiJob = false,
            PlannedOTJTrainingHours = 1200,
            StandardCode = 123,
            ConsumerReference = "AAAAA"
        };
    }

    private List<ValidationResult> RunValidation(LearnerDataRequest learner)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(learner);

        Validator.TryValidateObject(learner, validationContext, validationResults, true);

        return validationResults;
    }
}