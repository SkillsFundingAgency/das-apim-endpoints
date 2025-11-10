using Azure.Core;
using FluentAssertions;
using FluentValidation.Results;
using NUnit.Framework;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Validators;

namespace SFA.DAS.LearnerData.UnitTests.Validators;

public class WhenValidatingLearnerData
{
    private string _ukprn;
    private int _academicYear;

    [SetUp]
    public void SetUp()
    {
        _ukprn = "10001234";
        _academicYear = 2425;
    }

    [Test]
    public async Task And_when_AreAllValid_Then_No_Errors_returned()
    {
        var learner = CreateValidCreateLearnerRequest();
        var result = await RunValidation(learner);
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task And_when_ULN_IsNotValid_Then_BadRequest_returned()
    {
        var request = CreateValidCreateLearnerRequest();
        request.Learner.Uln = "1234";
        var result = await RunValidation(request);
        result.IsValid.Should().BeFalse();
        result.Errors.First().ErrorMessage.Should().Be($"Learner data contains incorrect ULN {request.Learner.Uln}");
        result.Errors.First().PropertyName.Should().Contain("Uln");
    }

    [Test]
    public async Task And_when_ConsumerReference_Is_Too_Big()
    {
        var request = CreateValidCreateLearnerRequest();
        request.ConsumerReference = new string('C', 101);
        var result = await RunValidation(request);
        result.IsValid.Should().BeFalse();
        result.Errors.First().PropertyName.Should().Contain("ConsumerReference");
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task And_Firstname_Is_blank(string? name)
    {
        var request = CreateValidCreateLearnerRequest();
        request.Learner.FirstName = name;
        var result = await RunValidation(request);
        result.IsValid.Should().BeFalse();
        result.Errors.First().PropertyName.Should().Contain("FirstName");
    }


    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task And_Lastname_Is_blank(string? name)
    {
        var request = CreateValidCreateLearnerRequest();
        request.Learner.LastName = name;
        var result = await RunValidation(request);
        result.IsValid.Should().BeFalse();
        result.Errors.First().PropertyName.Should().Contain("LastName");
    }

    [Test]
    public async Task And_EmailAddress_Is_Invalid()
    {
        var request = CreateValidCreateLearnerRequest();
        request.Learner.Email = "NotValids.com";
        var result = await RunValidation(request);
        result.IsValid.Should().BeFalse();
        result.Errors.First().PropertyName.Should().Contain("Learner.Email");
    }

    [Test]
    public async Task And_EmailAddress_Is_Null()
    {
        var request = CreateValidCreateLearnerRequest();
        request.Learner.Email = null;
        var result = await RunValidation(request);
        result.IsValid.Should().BeTrue();
    }

    [TestCase("")]
    [TestCase(" ")]
    public async Task And_EmailAddress_Is_blank(string email)
    {
        var request = CreateValidCreateLearnerRequest();
        request.Learner.Email = email;
        var result = await RunValidation(request);
        result.IsValid.Should().BeFalse();
        result.Errors.First().PropertyName.Should().Contain("Learner.Email");
    }

    [Test]
    public async Task And_EPAOPrice_Is_Negative()
    {
        var request = CreateValidCreateLearnerRequest();
        request.Delivery.OnProgramme.First().Costs.Single().EpaoPrice = -1;
        var result = await RunValidation(request);
        result.IsValid.Should().BeFalse();
        result.Errors.First().ErrorMessage.Should().Contain($"Learner data contains a negative EpaoPrice {request.Delivery.OnProgramme.First().Costs.Single().EpaoPrice}");
        result.Errors.First().PropertyName.Should().Contain("EpaoPrice");
    }

    [Test]
    public async Task And_TrainingPrice_Is_Negative()
    {
        var request = CreateValidCreateLearnerRequest();
        request.Delivery.OnProgramme.First().Costs.Single().TrainingPrice = -1;
        var result = await RunValidation(request);
        result.IsValid.Should().BeFalse();
        result.Errors.First().ErrorMessage.Should().Contain($"Learner data contains a negative TrainingPrice {request.Delivery.OnProgramme.First().Costs.Single().TrainingPrice}");
        result.Errors.First().PropertyName.Should().Contain("TrainingPrice");
    }

    [Test]
    public async Task And_StandardCode_Is_Negative()
    {
        var request = CreateValidCreateLearnerRequest();
        request.Delivery.OnProgramme.First().StandardCode = -1;
        var result = await RunValidation(request);
        result.IsValid.Should().BeFalse();
        result.Errors.First().ErrorMessage.Should().Contain($"Learner data contains a negative StandardCode {request.Delivery.OnProgramme.First().StandardCode}");
        result.Errors.First().PropertyName.Should().Contain("StandardCode");
    }

    [Test]
    public async Task And_FirstName_Too_Long()
    {
        var request = CreateValidCreateLearnerRequest();
        request.Learner.FirstName = new string('A', 101);
        var result = await RunValidation(request);
        result.IsValid.Should().BeFalse();
        result.Errors.First().ErrorMessage.Should().Be("Firstname cannot be more then 100 characters long");
        result.Errors.First().PropertyName.Should().Contain("Learner.FirstName");
    }

    [Test]
    public async Task And_LastName_Too_Long()
    {
        var request = CreateValidCreateLearnerRequest();
        request.Learner.LastName = new string('B', 101);
        var result = await RunValidation(request);
        result.IsValid.Should().BeFalse();
        result.Errors.First().ErrorMessage.Should().Be("Lastname cannot be more then 100 characters long");
        result.Errors.First().PropertyName.Should().Contain("Learner.LastName");
    }

    [Test]
    public async Task And_LearnerEmail_Too_Long()
    {
        var request = CreateValidCreateLearnerRequest();
        request.Learner.Email = new string('C', 201) + "@test.com";
        var result = await RunValidation(request);
        result.IsValid.Should().BeFalse();
        result.Errors.First().ErrorMessage.Should().Be("Email cannot be more then 200 characters long");
        result.Errors.First().PropertyName.Should().Contain("Learner.Email");
    }

    [Test]
    public async Task And_AgreementId_Too_Long()
    {
        var request = CreateValidCreateLearnerRequest();
        request.Delivery.OnProgramme.First().AgreementId = new string('D', 21);
        var result = await RunValidation(request);
        result.IsValid.Should().BeFalse();
        result.Errors.First().ErrorMessage.Should().Be("OnProgramme AgreementId cannot be more then 20 characters long");
        result.Errors.First().PropertyName.Should().Contain("AgreementId");
    }

    [Test]
    public async Task And_ConsumerReference_Too_Long()
    {
        var learner = CreateValidCreateLearnerRequest();
        learner.ConsumerReference = new string('E', 101);
        var result = await RunValidation(learner);
        result.IsValid.Should().BeFalse();
        result.Errors.First().ErrorMessage.Should().Be("ConsumerReference cannot be more then 100 characters long");
        result.Errors.First().PropertyName.Should().Contain("ConsumerReference");
    }

    private CreateLearnerRequest CreateValidCreateLearnerRequest()
    {
        return new CreateLearnerRequest
        {
            ConsumerReference = "AAAAA",
            Learner = new CreateLearnerRequest.LearnerDetails
            {
                Uln = "1234567890",
                FirstName = "First",
                LastName = "Last",
                Email = "Email@abcd.com",
                Dob = new DateTime(2000, 02, 01)
            },
            Delivery = new CreateLearnerRequest.DeliveryDetails
            {
                OnProgramme = new List<CreateLearnerRequest.OnProgrammeDetails>
                {
                    new CreateLearnerRequest.OnProgrammeDetails
                    {
                        AgreementId = "ABCD",
                        IsFlexiJob = false,
                        StartDate = new DateTime(2025, 02, 01),
                        ExpectedEndDate = new DateTime(2027, 02, 01),
                        StandardCode = 123,
                        Costs = new List<CostDetails>
                        {
                            new CostDetails
                            {
                                EpaoPrice = 400,
                                TrainingPrice = 3200
                            }
                        }
                    }
                }
            }
        };
    }

    private Task<ValidationResult> RunValidation(CreateLearnerRequest request)
    {
        var sut = new CreateLearnerRequestValidator();

        return sut.ValidateAsync(request);
    }
}