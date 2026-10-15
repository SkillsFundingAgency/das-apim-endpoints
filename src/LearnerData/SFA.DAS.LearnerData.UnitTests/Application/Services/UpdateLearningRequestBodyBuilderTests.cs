using AutoFixture;
using SFA.DAS.Common.Domain.Types;
using SFA.DAS.LearnerData.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using SFA.DAS.LearnerData.Requests;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services;

[TestFixture]
public class UpdateLearningRequestBodyBuilderTests
{
    [Test]
    public async Task Build_Sets_WithdrawalDate_From_LatestOnProgramme()
    {
        var fixture = new Fixture();

        // Arrange
        var command = BreaksInLearningTestHelper.CreateLearnerWithBreaksInLearning(false);
        var withdrawalDate = fixture.Create<DateTime>();
        command.UpdateLearnerRequest.Delivery.OnProgramme.Last().WithdrawalDate = withdrawalDate;

        var sut = CreateSut(Mock.Of<IEnglishAndMathsValueService>());

        // Act
        var actualRequestBody = await sut.Build(command.Ukprn, command.UpdateLearnerRequest);

        // Assert
        actualRequestBody.OnProgramme.WithdrawalDate.Should().Be(withdrawalDate);
    }

    [Test]
    public async Task Build_Sets_CompletionDate_From_LatestOnProgramme()
    {
        var fixture = new Fixture();

        // Arrange
        var command = BreaksInLearningTestHelper.CreateLearnerWithBreaksInLearning(false);
        var completionDate = fixture.Create<DateTime>();
        command.UpdateLearnerRequest.Delivery.OnProgramme.Last().CompletionDate = completionDate;

        var sut = CreateSut(Mock.Of<IEnglishAndMathsValueService>());

        // Act
        var actualRequestBody = await sut.Build(command.Ukprn, command.UpdateLearnerRequest);

        // Assert
        actualRequestBody.Learner.CompletionDate.Should().Be(completionDate);
    }

    [Test]
    public async Task Build_Sets_ExpectedEndDate_From_LatestOnProgramme()
    {
        var fixture = new Fixture();

        // Arrange
        var command = BreaksInLearningTestHelper.CreateLearnerWithBreaksInLearning(false);
        var expectedEndDate = fixture.Create<DateTime>();
        command.UpdateLearnerRequest.Delivery.OnProgramme.Last().ExpectedEndDate = expectedEndDate;

        var sut = CreateSut(Mock.Of<IEnglishAndMathsValueService>());

        // Act
        var actualRequestBody = await sut.Build(command.Ukprn, command.UpdateLearnerRequest);

        // Assert
        actualRequestBody.OnProgramme.ExpectedEndDate.Should().Be(expectedEndDate);
    }

    [Test]
    public async Task Build_Sets_AchievementDate_From_LatestOnProgramme()
    {
        var fixture = new Fixture();

        // Arrange
        var command = BreaksInLearningTestHelper.CreateLearnerWithBreaksInLearning(false);
        var achievementDate = fixture.Create<DateTime>();
        command.UpdateLearnerRequest.Delivery.OnProgramme.Last().AchievementDate = achievementDate;

        var sut = CreateSut(Mock.Of<IEnglishAndMathsValueService>());

        // Act
        var actualRequestBody = await sut.Build(command.Ukprn, command.UpdateLearnerRequest);

        // Assert
        actualRequestBody.OnProgramme.AchievementDate.Should().Be(achievementDate);
    }

    [Test]
    public async Task Build_Sets_PauseDate_From_LatestOnProgramme()
    {
        var fixture = new Fixture();

        // Arrange
        var command = BreaksInLearningTestHelper.CreateLearnerWithBreaksInLearning(false);
        var pauseDate = fixture.Create<DateTime>();
        command.UpdateLearnerRequest.Delivery.OnProgramme.Last().PauseDate = pauseDate;

        var sut = CreateSut(Mock.Of<IEnglishAndMathsValueService>());

        // Act
        var actualRequestBody = await sut.Build(command.Ukprn, command.UpdateLearnerRequest);

        // Assert
        actualRequestBody.OnProgramme.PauseDate.Should().Be(pauseDate);
    }

    [Test]
    public async Task Build_Sets_CareDetails_From_LatestOnProgramme()
    {
        var fixture = new Fixture();

        // Arrange
        var command = BreaksInLearningTestHelper.CreateLearnerWithBreaksInLearning(false);
        var careDetails = fixture.Create<Requests.Care>();
        command.UpdateLearnerRequest.Delivery.OnProgramme.Last().Care = careDetails;

        var sut = CreateSut(Mock.Of<IEnglishAndMathsValueService>());

        // Act
        var actualRequestBody = await sut.Build(command.Ukprn, command.UpdateLearnerRequest);

        // Assert
        actualRequestBody.Learner.Care.HasEHCP.Should().Be(command.UpdateLearnerRequest.Learner.HasEhcp);
        actualRequestBody.Learner.Care.IsCareLeaver.Should().Be(careDetails.Careleaver);
        actualRequestBody.Learner.Care.CareLeaverEmployerConsentGiven.Should().Be(careDetails.EmployerConsent);
    }

    [Test]
    public async Task Build_WithCreateLearnerRequest_MapsPropertiesCorrectly()
    {
        var fixture = new Fixture();

        // Arrange
        var createRequest = new CreateLearnerRequest
        {
            Learner = new CreateLearnerRequest.LearnerDetails
            {
                Uln = 123456789,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@test.com",
                Dob = new DateTime(2000, 1, 1),
                HasEhcp = true
            },
            Delivery = new CreateLearnerRequest.DeliveryDetails
            {
                OnProgramme = new List<CreateLearnerRequest.OnProgrammeDetails>
                {
                    new CreateLearnerRequest.OnProgrammeDetails
                    {
                        StartDate = new DateTime(2023, 9, 1),
                        ExpectedEndDate = new DateTime(2025, 9, 1),
                        StandardCode = 123,
                        AgreementId = "AG-1",
                        Costs = new List<CostDetails>
                        {
                            new CostDetails
                            {
                                FromDate = new DateTime(2023, 9, 1),
                                TrainingPrice = 5000,
                                EpaoPrice = 1000
                            }
                        },
                        Care = new Care
                        {
                            Careleaver = true,
                            EmployerConsent = true
                        }
                    }
                },
                EnglishAndMaths = new List<MathsAndEnglish>()
            }
        };

        var sut = CreateSut(Mock.Of<IEnglishAndMathsValueService>());

        // Act
        var actualRequestBody = await sut.Build(10005077, createRequest, 2526, LearningType.Apprenticeship);

        // Assert
        actualRequestBody.Learner.Uln.Should().Be(123456789);
        actualRequestBody.Learner.FirstName.Should().Be("John");
        actualRequestBody.Learner.LastName.Should().Be("Doe");
        actualRequestBody.Learner.EmailAddress.Should().Be("john.doe@test.com");
        actualRequestBody.Learner.DateOfBirth.Should().Be(new DateTime(2000, 1, 1));
        actualRequestBody.Learner.Care.HasEHCP.Should().BeTrue();
        actualRequestBody.Learner.Care.IsCareLeaver.Should().BeTrue();
        actualRequestBody.Learner.Care.CareLeaverEmployerConsentGiven.Should().BeTrue();
        actualRequestBody.OnProgramme.TrainingCode.Should().Be("123");
        actualRequestBody.AcademicYear.Should().Be(2526);
        actualRequestBody.Delivery.LearningType.Should().Be(LearningType.Apprenticeship);
    }

    [Test]
    public async Task Build_Sets_LearnerRef_From_UpdateLearnerRequest()
    {
        // Arrange
        var command = BreaksInLearningTestHelper.CreateLearnerWithBreaksInLearning(false);
        command.UpdateLearnerRequest.Learner.LearnerRef = "LR-12345";

        var sut = CreateSut(Mock.Of<IEnglishAndMathsValueService>());

        // Act
        var actualRequestBody = await sut.Build(command.Ukprn, command.UpdateLearnerRequest);

        // Assert
        actualRequestBody.Learner.LearnerRef.Should().Be("LR-12345");
    }

    [TestCase(null)]
    [TestCase("")]
    public async Task Build_Does_Not_Throw_When_LearnerRef_Is_Null_Or_Empty_On_UpdateLearnerRequest(string? learnerRef)
    {
        // Arrange
        var command = BreaksInLearningTestHelper.CreateLearnerWithBreaksInLearning(false);
        command.UpdateLearnerRequest.Learner.LearnerRef = learnerRef!;

        var sut = CreateSut(Mock.Of<IEnglishAndMathsValueService>());

        // Act
        Func<Task> act = () => sut.Build(command.Ukprn, command.UpdateLearnerRequest);

        // Assert
        await act.Should().NotThrowAsync();
        var actualRequestBody = await sut.Build(command.Ukprn, command.UpdateLearnerRequest);
        actualRequestBody.Learner.LearnerRef.Should().Be(learnerRef);
    }

    [Test]
    public async Task Build_WithCreateLearnerRequest_MapsLearnerRef()
    {
        // Arrange
        var createRequest = new CreateLearnerRequest
        {
            Learner = new CreateLearnerRequest.LearnerDetails
            {
                Uln = 123456789,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@test.com",
                Dob = new DateTime(2000, 1, 1),
                HasEhcp = true,
                LearnerRef = "LR-98765"
            },
            Delivery = new CreateLearnerRequest.DeliveryDetails
            {
                OnProgramme = new List<CreateLearnerRequest.OnProgrammeDetails>
                {
                    new CreateLearnerRequest.OnProgrammeDetails
                    {
                        StartDate = new DateTime(2023, 9, 1),
                        ExpectedEndDate = new DateTime(2025, 9, 1),
                        StandardCode = 123,
                        AgreementId = "AG-1",
                        Costs = new List<CostDetails>
                        {
                            new CostDetails
                            {
                                FromDate = new DateTime(2023, 9, 1),
                                TrainingPrice = 5000,
                                EpaoPrice = 1000
                            }
                        },
                        Care = new Care
                        {
                            Careleaver = true,
                            EmployerConsent = true
                        }
                    }
                },
                EnglishAndMaths = new List<MathsAndEnglish>()
            }
        };

        var sut = CreateSut(Mock.Of<IEnglishAndMathsValueService>());

        // Act
        var actualRequestBody = await sut.Build(10005077, createRequest, 2526, LearningType.Apprenticeship);

        // Assert
        actualRequestBody.Learner.LearnerRef.Should().Be("LR-98765");
    }

    [TestCase(null)]
    [TestCase("")]
    public async Task Build_WithCreateLearnerRequest_Does_Not_Throw_When_LearnerRef_Is_Null_Or_Empty(string? learnerRef)
    {
        // Arrange
        var createRequest = new CreateLearnerRequest
        {
            Learner = new CreateLearnerRequest.LearnerDetails
            {
                Uln = 123456789,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@test.com",
                Dob = new DateTime(2000, 1, 1),
                HasEhcp = true,
                LearnerRef = learnerRef!
            },
            Delivery = new CreateLearnerRequest.DeliveryDetails
            {
                OnProgramme = new List<CreateLearnerRequest.OnProgrammeDetails>
                {
                    new CreateLearnerRequest.OnProgrammeDetails
                    {
                        StartDate = new DateTime(2023, 9, 1),
                        ExpectedEndDate = new DateTime(2025, 9, 1),
                        StandardCode = 123,
                        AgreementId = "AG-1",
                        Costs = new List<CostDetails>
                        {
                            new CostDetails
                            {
                                FromDate = new DateTime(2023, 9, 1),
                                TrainingPrice = 5000,
                                EpaoPrice = 1000
                            }
                        },
                        Care = new Care
                        {
                            Careleaver = true,
                            EmployerConsent = true
                        }
                    }
                },
                EnglishAndMaths = new List<MathsAndEnglish>()
            }
        };

        var sut = CreateSut(Mock.Of<IEnglishAndMathsValueService>());

        // Act
        Func<Task> act = () => sut.Build(10005077, createRequest, 2526, LearningType.Apprenticeship);

        // Assert
        await act.Should().NotThrowAsync();
        var actualRequestBody = await sut.Build(10005077, createRequest, 2526, LearningType.Apprenticeship);
        actualRequestBody.Learner.LearnerRef.Should().Be(learnerRef);
    }

    [Test]
    public async Task Build_Sets_TrainingCode_From_FirstOnProgramme_StandardCode()
    {
        var fixture = new Fixture();

        // Arrange
        var command = BreaksInLearningTestHelper.CreateLearnerWithBreaksInLearning(false);
        var firstOnProgramme = command.UpdateLearnerRequest.Delivery.OnProgramme
            .OrderBy(x => x.StartDate)
            .First();

        var sut = CreateSut(Mock.Of<IEnglishAndMathsValueService>());

        // Act
        var actualRequestBody = await sut.Build(command.Ukprn, command.UpdateLearnerRequest);

        // Assert
        actualRequestBody.OnProgramme.TrainingCode.Should().Be(firstOnProgramme.StandardCode.ToString());
    }

    [Test]
    public async Task Build_Sets_EnglishAndMaths_Amount_To_The_Value_From_Sld_Not_The_Inbound_Amount()
    {
        // Arrange
        var command = BreaksInLearningTestHelper.CreateLearnerWithBreaksInLearning(false);
        command.UpdateLearnerRequest.Delivery.EnglishAndMaths =
        [
            EnglishAndMathsCourse("60342287", new DateTime(2025, 9, 1), inboundAmount: 999m)
        ];

        var englishAndMathsValueService = new Mock<IEnglishAndMathsValueService>();
        englishAndMathsValueService.Setup(x => x.GetValue("60342287", new DateTime(2025, 9, 1))).ReturnsAsync(1234.56m);

        var sut = CreateSut(englishAndMathsValueService.Object);

        // Act
        var actualRequestBody = await sut.Build(command.Ukprn, command.UpdateLearnerRequest);

        // Assert
        actualRequestBody.EnglishAndMathsCourses.Single().Amount.Should().Be(1234.56m);
    }

    [Test]
    public async Task Build_Looks_Up_Each_EnglishAndMaths_Course_Separately()
    {
        // Arrange
        var command = BreaksInLearningTestHelper.CreateLearnerWithBreaksInLearning(false);
        command.UpdateLearnerRequest.Delivery.EnglishAndMaths =
        [
            EnglishAndMathsCourse("60342287", new DateTime(2025, 9, 1)),
            EnglishAndMathsCourse("50086832", new DateTime(2025, 10, 1))
        ];

        var englishAndMathsValueService = new Mock<IEnglishAndMathsValueService>();
        englishAndMathsValueService.Setup(x => x.GetValue("60342287", new DateTime(2025, 9, 1))).ReturnsAsync(1000m);
        englishAndMathsValueService.Setup(x => x.GetValue("50086832", new DateTime(2025, 10, 1))).ReturnsAsync(1500m);

        var sut = CreateSut(englishAndMathsValueService.Object);

        // Act
        var actualRequestBody = await sut.Build(command.Ukprn, command.UpdateLearnerRequest);

        // Assert
        actualRequestBody.EnglishAndMathsCourses.Single(x => x.LearnAimRef == "60342287").Amount.Should().Be(1000m);
        actualRequestBody.EnglishAndMathsCourses.Single(x => x.LearnAimRef == "50086832").Amount.Should().Be(1500m);
    }

    [Test]
    public async Task Build_Looks_Up_A_Course_With_A_Break_In_Learning_Once_Using_Its_First_StartDate()
    {
        // Arrange
        var command = BreaksInLearningTestHelper.CreateLearnerWithBreaksInLearning(false);
        command.UpdateLearnerRequest.Delivery.EnglishAndMaths =
        [
            EnglishAndMathsCourse("60342287", new DateTime(2026, 1, 1)),
            EnglishAndMathsCourse("60342287", new DateTime(2025, 9, 1))
        ];

        var englishAndMathsValueService = new Mock<IEnglishAndMathsValueService>();
        englishAndMathsValueService.Setup(x => x.GetValue("60342287", new DateTime(2025, 9, 1))).ReturnsAsync(1234.56m);

        var sut = CreateSut(englishAndMathsValueService.Object);

        // Act
        var actualRequestBody = await sut.Build(command.Ukprn, command.UpdateLearnerRequest);

        // Assert
        actualRequestBody.EnglishAndMathsCourses.Single().Amount.Should().Be(1234.56m);
        englishAndMathsValueService.Verify(x => x.GetValue(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once);
    }

    [Test]
    public async Task Build_WithCreateLearnerRequest_Sets_EnglishAndMaths_Amount_To_The_Value_From_Sld()
    {
        // Arrange
        var createRequest = CreateLearnerRequestWithEnglishAndMaths(
            EnglishAndMathsCourse("60342287", new DateTime(2025, 9, 1), inboundAmount: 999m));

        var englishAndMathsValueService = new Mock<IEnglishAndMathsValueService>();
        englishAndMathsValueService.Setup(x => x.GetValue("60342287", new DateTime(2025, 9, 1))).ReturnsAsync(1234.56m);

        var sut = CreateSut(englishAndMathsValueService.Object);

        // Act
        var actualRequestBody = await sut.Build(10005077, createRequest, 2526, LearningType.Apprenticeship);

        // Assert
        actualRequestBody.EnglishAndMathsCourses.Single().Amount.Should().Be(1234.56m);
    }

    [Test]
    public async Task Build_When_The_EnglishAndMaths_Value_Cannot_Be_Obtained_Lets_The_Failure_Through()
    {
        // Arrange
        var command = BreaksInLearningTestHelper.CreateLearnerWithBreaksInLearning(false);
        command.UpdateLearnerRequest.Delivery.EnglishAndMaths =
        [
            EnglishAndMathsCourse("60342287", new DateTime(2025, 9, 1))
        ];

        var englishAndMathsValueService = new Mock<IEnglishAndMathsValueService>();
        englishAndMathsValueService
            .Setup(x => x.GetValue(It.IsAny<string>(), It.IsAny<DateTime>()))
            .ThrowsAsync(new EnglishAndMathsValueNotFoundException("60342287", new DateTime(2025, 9, 1)));

        var sut = CreateSut(englishAndMathsValueService.Object);

        // Act
        var act = () => sut.Build(command.Ukprn, command.UpdateLearnerRequest);

        // Assert
        await act.Should().ThrowAsync<EnglishAndMathsValueNotFoundException>();
    }

    private static UpdateLearningRequestBodyBuilder CreateSut(IEnglishAndMathsValueService englishAndMathsValueService)
    {
        return new UpdateLearningRequestBodyBuilder(
            Mock.Of<ILearningSupportService>(),
            Mock.Of<IBreaksInLearningService>(),
            Mock.Of<ICostsService>(),
            englishAndMathsValueService);
    }

    private static MathsAndEnglish EnglishAndMathsCourse(string learnAimRef, DateTime startDate, decimal inboundAmount = 0)
    {
        return new MathsAndEnglish
        {
            LearnAimRef = learnAimRef,
            Course = "Maths",
            Amount = inboundAmount,
            StartDate = startDate,
            EndDate = startDate.AddYears(1),
            LearningSupport = []
        };
    }

    private static CreateLearnerRequest CreateLearnerRequestWithEnglishAndMaths(params MathsAndEnglish[] englishAndMaths)
    {
        return new CreateLearnerRequest
        {
            Learner = new CreateLearnerRequest.LearnerDetails
            {
                Uln = 123456789,
                Dob = new DateTime(2000, 1, 1)
            },
            Delivery = new CreateLearnerRequest.DeliveryDetails
            {
                OnProgramme =
                [
                    new CreateLearnerRequest.OnProgrammeDetails
                    {
                        StartDate = new DateTime(2025, 9, 1),
                        ExpectedEndDate = new DateTime(2027, 9, 1),
                        StandardCode = 123,
                        AgreementId = "AG-1",
                        Costs = [new CostDetails { FromDate = new DateTime(2025, 9, 1), TrainingPrice = 5000, EpaoPrice = 1000 }],
                        Care = new Care()
                    }
                ],
                EnglishAndMaths = englishAndMaths.ToList()
            }
        };
    }
}
