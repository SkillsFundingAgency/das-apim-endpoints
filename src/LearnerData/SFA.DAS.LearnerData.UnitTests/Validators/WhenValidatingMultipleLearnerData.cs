using FluentAssertions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Validators;

namespace SFA.DAS.LearnerData.UnitTests.Validators;

public class WhenValidatingMultipleLearnerData
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
        List<LearnerDataRequest> learners = new()
        {
            CreateValidLearnerDataRequest(),
            CreateValidLearnerDataRequest(),
            CreateValidLearnerDataRequest()
        };
        var result = await RunValidation(learners);
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task And_when_UkprnNotMatch_Then_Errors_returned()
    {
        List<LearnerDataRequest> learners = new()
        {
            CreateValidLearnerDataRequest(),
            CreateValidLearnerDataRequest(),
            CreateValidLearnerDataRequest()
        };
        learners[0].UKPRN = _ukprn + 100;

        var result = await RunValidation(learners);
        result.IsValid.Should().BeFalse();
        result.Errors.First().ErrorMessage.Should().Be($"Learner data contains different UKPRN to {_ukprn}");
        result.Errors.First().PropertyName.Should().Contain("x[0].UKPRN");
    }

    //[Test]
    //public async Task And_when_StandardCode_And_LarsCode_Is_Missing_Then_Error_Returned()
    //{
    //    List<LearnerDataRequest> learners = new()
    //    {
    //        CreateValidLearnerDataRequest(),
    //        CreateValidLearnerDataRequest(),
    //        CreateValidLearnerDataRequest()
    //    };
    //    learners[0].StandardCode = null;
    //    learners[0].LarsCode = null;

    //    var result = await RunValidation(learners);
    //    result.IsValid.Should().BeFalse();
    //    result.Errors.First().ErrorMessage.Should().Be("Learner data must contains a LarsCode when StandardCode is null");
    //    result.Errors.First().PropertyName.Should().Contain("x[0].LarsCode");
    //}

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

    private Task<ValidationResult> RunValidation(List<LearnerDataRequest> learners)
    {
        Mock<IHttpContextAccessor> accessorMock = new Mock<IHttpContextAccessor>();

        var sut = new BulkLearnerDataRequestsValidator(BuildMockHttpContextAccessor().Object);

        return sut.ValidateAsync(learners);
    }

    private Mock<IHttpContextAccessor> BuildMockHttpContextAccessor()
    {
        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

        var mockHttpContext = new DefaultHttpContext();
        mockHttpContext.Request.RouteValues = new RouteValueDictionary
        {
            { "ukprn", _ukprn.ToString()},
            { "academicyear", _academicYear.ToString()}
        };
        mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(mockHttpContext);

        return mockHttpContextAccessor;
    }
}