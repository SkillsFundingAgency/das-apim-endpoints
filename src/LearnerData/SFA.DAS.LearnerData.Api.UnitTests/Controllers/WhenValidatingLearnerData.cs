using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.LearnerData.Requests;

namespace SFA.DAS.LearnerData.Api.UnitTests.Controllers;

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

    [TestCase(-1)]
    [TestCase(9999999)]
    [TestCase(1000000000)]
    public void And_UKPRN_Is_OutOfRange(int ukprn)
    {
        var learner = CreateValidLearnerDataRequest();
        learner.UKPRN = ukprn;
        _ukprn = ukprn;
        var validationResults = RunValidation(learner);
        validationResults.Any().Should().BeTrue();
        validationResults.First().ErrorMessage.Should().Be($"Learner data contains incorrect UKPRN {learner.UKPRN}");
        validationResults.First().MemberNames.Should().Contain("UKPRN");
    }

    [Test]
    public void And_when_UKPRN_Does_Not_Match_Url()
    {
        var learner = CreateValidLearnerDataRequest();
        learner.UKPRN = 12345678;
        _ukprn = 10001234;
        var validationResults = RunValidation(learner);
        validationResults.Any().Should().BeTrue();
        validationResults.First().ErrorMessage.Should().Be($"Learner data contains different UKPRN to {_ukprn}");
        validationResults.First().MemberNames.Should().Contain("UKPRN");
    }

    [Test]
    public void And_when_StartDate_Is_Not_In_AcademicYear()
    {
        var learner = CreateValidLearnerDataRequest();
        learner.StartDate = DateTime.Today; 
        _academicYear = 2324;
        var validationResults = RunValidation(learner);
        validationResults.Any().Should().BeTrue();
        validationResults.First().ErrorMessage.Should().Be($"Learner data contains a StartDate {learner.StartDate} that is not in the academic year {_academicYear}");
        validationResults.First().MemberNames.Should().Contain("StartDate");
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
        learner.LearnerEmail = "NotValids.com";
        var validationResults = RunValidation(learner);
        validationResults.Any().Should().BeTrue();
        validationResults.First().MemberNames.Should().Contain("LearnerEmail");
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

    private List<ValidationResult> RunValidation(LearnerDataRequest learner)
    {
        var validationResults = new List<ValidationResult>();
        var mockServiceProvider = GetMockServiceProvider();
        var validationContext = new ValidationContext(learner, mockServiceProvider, items: null);

        Validator.TryValidateObject(learner, validationContext, validationResults, true);

        return validationResults;
    }

    private IServiceProvider GetMockServiceProvider()
    {
        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

        var mockHttpContext = new DefaultHttpContext();
        mockHttpContext.Request.RouteValues = new RouteValueDictionary
        {
            { "ukprn", _ukprn.ToString()},
            { "academicyear", _academicYear.ToString()}
        };
        //mockHttpContext.Request.Headers["Authorization"] = "Bearer fake-token";
        mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(mockHttpContext);

        var mockServiceProvider = new Mock<IServiceProvider>();
        mockServiceProvider.Setup(_ => _.GetService(typeof(IHttpContextAccessor)))
            .Returns(mockHttpContextAccessor.Object);

        return mockServiceProvider.Object;
    }
}