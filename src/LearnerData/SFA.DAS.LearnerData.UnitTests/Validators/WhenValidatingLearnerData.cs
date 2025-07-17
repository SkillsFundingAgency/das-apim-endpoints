using FluentAssertions;
using FluentValidation.Results;
using NUnit.Framework;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Validators;

namespace SFA.DAS.LearnerData.UnitTests.Validators;

public class WhenValidatingLearnerData
{
    private long _ukprn;
    private int _academicYear;

    [SetUp]
    public void SetUp()
    {
        _ukprn = 10001234;
        _academicYear = 2425;
    }

    [Test]
    public async Task And_when_AreAllValid_Then_No_Errors_returned()
    {
        var learner = CreateValidLearnerDataRequest();
        var result = await RunValidation(learner);
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task And_when_ULN_IsNotValid_Then_BadRequest_returned()
    {
        var learner = CreateValidLearnerDataRequest();
        learner.ULN = 1234;
        var result = await RunValidation(learner);
        result.IsValid.Should().BeFalse();
        result.Errors.First().ErrorMessage.Should().Be($"Learner data contains incorrect ULN {learner.ULN}");
        result.Errors.First().PropertyName.Should().Contain("ULN");
    }

    [TestCase(-1)]
    [TestCase(9999999)]
    [TestCase(10000000000)]
    public async Task And_UKPRN_Is_OutOfRange(long ukprn)
    {
        var learner = CreateValidLearnerDataRequest();
        learner.UKPRN = ukprn;
        _ukprn = ukprn;
        var result = await RunValidation(learner);
        result.IsValid.Should().BeFalse();
        result.Errors.First().ErrorMessage.Should().Be($"Learner data contains incorrect UKPRN {learner.UKPRN}");
        result.Errors.First().PropertyName.Should().Contain("UKPRN");
    }

    [Test]
    public async Task And_when_UKPRN_Does_Not_Match_Url()
    {
        var learner = CreateValidLearnerDataRequest();
        learner.UKPRN = 12345678;
        _ukprn = 10001234;
        var result = await RunValidation(learner);
        result.IsValid.Should().BeFalse();
        result.Errors.First().ErrorMessage.Should().Be($"Learner data contains different UKPRN to {_ukprn}");
        result.Errors.First().PropertyName.Should().Contain("UKPRN");
    }

    // [Test]
    // public async Task And_when_StartDate_Is_Not_In_AcademicYear()
    // {
    //     var learner = CreateValidLearnerDataRequest();
    //     learner.StartDate = DateTime.Today;
    //     _academicYear = 2324;
    //     var result = await RunValidation(learner);
    //     result.IsValid.Should().BeFalse();
    //     result.Errors.First().ErrorMessage.Should().Be($"Learner data contains a StartDate {learner.StartDate} that is not in the academic year {_academicYear}");
    //     result.Errors.First().PropertyName.Should().Contain("StartDate");
    // }

    [Test]
    public async Task And_when_ConsumerReference_Is_Too_Big()
    {
        var learner = CreateValidLearnerDataRequest();
        learner.ConsumerReference = new string('C', 101);
        var result = await RunValidation(learner);
        result.IsValid.Should().BeFalse();
        result.Errors.First().PropertyName.Should().Contain("ConsumerReference");
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task And_Firstname_Is_blank(string? name)
    {
        var learner = CreateValidLearnerDataRequest();
        learner.FirstName = name;
        var result = await RunValidation(learner);
        result.IsValid.Should().BeFalse();
        result.Errors.First().PropertyName.Should().Contain("FirstName");
    }


    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task And_Lastname_Is_blank(string? name)
    {
        var learner = CreateValidLearnerDataRequest();
        learner.LastName = name;
        var result = await RunValidation(learner);
        result.IsValid.Should().BeFalse();
        result.Errors.First().PropertyName.Should().Contain("LastName");
    }

    [Test]
    public async Task And_EmailAddress_Is_Invalid()
    {
        var learner = CreateValidLearnerDataRequest();
        learner.LearnerEmail = "NotValids.com";
        var result = await RunValidation(learner);
        result.IsValid.Should().BeFalse();
        result.Errors.First().PropertyName.Should().Contain("LearnerEmail");
    }

    [Test]
    public async Task And_EmailAddress_Is_Null()
    {
        var learner = CreateValidLearnerDataRequest();
        learner.LearnerEmail = null;
        var result = await RunValidation(learner);
        result.IsValid.Should().BeTrue();
    }

    [TestCase("")]
    [TestCase(" ")]
    public async Task And_EmailAddress_Is_blank(string email)
    {
        var learner = CreateValidLearnerDataRequest();
        learner.LearnerEmail = email;
        var result = await RunValidation(learner);
        result.IsValid.Should().BeFalse();
        result.Errors.First().PropertyName.Should().Contain("LearnerEmail");
    }

    [Test]
    public async Task And_EPAOPrice_Is_Negative()
    {
        var learner = CreateValidLearnerDataRequest();
        learner.EpaoPrice = -1;
        var result = await RunValidation(learner);
        result.IsValid.Should().BeFalse();
        result.Errors.First().ErrorMessage.Should().Contain($"Learner data contains a negative EpaoPrice {learner.EpaoPrice}");
        result.Errors.First().PropertyName.Should().Contain("EpaoPrice");
    }

    [Test]
    public async Task And_TrainingPrice_Is_Negative()
    {
        var learner = CreateValidLearnerDataRequest();
        learner.TrainingPrice = -1;
        var result = await RunValidation(learner);
        result.IsValid.Should().BeFalse();
        result.Errors.First().ErrorMessage.Should().Contain($"Learner data contains a negative TrainingPrice {learner.TrainingPrice}");
        result.Errors.First().PropertyName.Should().Contain("TrainingPrice");
    }

    [Test]
    public async Task And_PlannedOTJTrainingHours_Is_Negative()
    {
        var learner = CreateValidLearnerDataRequest();
        learner.PlannedOTJTrainingHours = -1;
        var result = await RunValidation(learner);
        result.IsValid.Should().BeFalse();
        result.Errors.First().ErrorMessage.Should().Contain($"Learner data contains a negative PlannedOTJTrainingHours {learner.PlannedOTJTrainingHours}");
        result.Errors.First().PropertyName.Should().Contain("PlannedOTJTrainingHours");
    }

    [Test]
    public async Task And_StandardCode_Is_Negative()
    {
        var learner = CreateValidLearnerDataRequest();
        learner.StandardCode = -1;
        var result = await RunValidation(learner);
        result.IsValid.Should().BeFalse();
        result.Errors.First().ErrorMessage.Should().Contain($"Learner data contains a negative StandardCode {learner.StandardCode}");
        result.Errors.First().PropertyName.Should().Contain("StandardCode");
    }

    [Test]
    public async Task And_FirstName_Too_Long()
    {
        var learner = CreateValidLearnerDataRequest();
        learner.FirstName = new string('A', 101);
        var result = await RunValidation(learner);
        result.IsValid.Should().BeFalse();
        result.Errors.First().ErrorMessage.Should().Be("FirstName cannot be more then 100 characters long");
        result.Errors.First().PropertyName.Should().Contain("FirstName");
    }

    [Test]
    public async Task And_LastName_Too_Long()
    {
        var learner = CreateValidLearnerDataRequest();
        learner.LastName = new string('B', 101);
        var result = await RunValidation(learner);
        result.IsValid.Should().BeFalse();
        result.Errors.First().ErrorMessage.Should().Be("LastName cannot be more then 100 characters long");
        result.Errors.First().PropertyName.Should().Contain("LastName");
    }

    [Test]
    public async Task And_LearnerEmail_Too_Long()
    {
        var learner = CreateValidLearnerDataRequest();
        learner.LearnerEmail = new string('C', 201) + "@test.com";
        var result = await RunValidation(learner);
        result.IsValid.Should().BeFalse();
        result.Errors.First().ErrorMessage.Should().Be("Email cannot be more then 200 characters long");
        result.Errors.First().PropertyName.Should().Contain("LearnerEmail");
    }

    [Test]
    public async Task And_AgreementId_Too_Long()
    {
        var learner = CreateValidLearnerDataRequest();
        learner.AgreementId = new string('D', 21);
        var result = await RunValidation(learner);
        result.IsValid.Should().BeFalse();
        result.Errors.First().ErrorMessage.Should().Be("AgreementId cannot be more then 20 characters long");
        result.Errors.First().PropertyName.Should().Contain("AgreementId");
    }

    [Test]
    public async Task And_ConsumerReference_Too_Long()
    {
        var learner = CreateValidLearnerDataRequest();
        learner.ConsumerReference = new string('E', 101);
        var result = await RunValidation(learner);
        result.IsValid.Should().BeFalse();
        result.Errors.First().ErrorMessage.Should().Be("ConsumerReference cannot be more then 100 characters long");
        result.Errors.First().PropertyName.Should().Contain("ConsumerReference");
    }

    private LearnerDataRequest CreateValidLearnerDataRequest()
    {
        return new LearnerDataRequest
        {
            ULN = 1234567890,
            UKPRN = _ukprn,
            FirstName = "First",
            LastName = "Last",
            LearnerEmail = "Email@abcd.com",
            DateOfBirth = new DateTime(2000, 02, 01),
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

    private Task<ValidationResult> RunValidation(LearnerDataRequest learner)
    {
        var sut = new LearnerDataRequestValidator(_ukprn, _academicYear);

        return sut.ValidateAsync(learner);
    }
}