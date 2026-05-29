using AutoFixture;
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
    public void Build_Sets_WithdrawalDate_From_LatestOnProgramme()
    {
        var fixture = new Fixture();

        // Arrange
        var command = BreaksInLearningTestHelper.CreateLearnerWithBreaksInLearning(false);
        var withdrawalDate = fixture.Create<DateTime>();
        command.UpdateLearnerRequest.Delivery.OnProgramme.Last().WithdrawalDate = withdrawalDate;

        var sut = new UpdateLearningRequestBodyBuilder(
            Mock.Of<ILearningSupportService>(),
            Mock.Of<IBreaksInLearningService>(),
            Mock.Of<ICostsService>());

        // Act
        var actualRequestBody = sut.Build(command.Ukprn, command.UpdateLearnerRequest);

        // Assert
        actualRequestBody.Delivery.WithdrawalDate.Should().Be(withdrawalDate);
    }

    [Test]
    public void Build_Sets_CompletionDate_From_LatestOnProgramme()
    {
        var fixture = new Fixture();

        // Arrange
        var command = BreaksInLearningTestHelper.CreateLearnerWithBreaksInLearning(false);
        var completionDate = fixture.Create<DateTime>();
        command.UpdateLearnerRequest.Delivery.OnProgramme.Last().CompletionDate = completionDate;

        var sut = new UpdateLearningRequestBodyBuilder(
            Mock.Of<ILearningSupportService>(),
            Mock.Of<IBreaksInLearningService>(),
            Mock.Of<ICostsService>());

        // Act
        var actualRequestBody = sut.Build(command.Ukprn, command.UpdateLearnerRequest);

        // Assert
        actualRequestBody.Learner.CompletionDate.Should().Be(completionDate);
    }

    [Test]
    public void Build_Sets_ExpectedEndDate_From_LatestOnProgramme()
    {
        var fixture = new Fixture();

        // Arrange
        var command = BreaksInLearningTestHelper.CreateLearnerWithBreaksInLearning(false);
        var expectedEndDate = fixture.Create<DateTime>();
        command.UpdateLearnerRequest.Delivery.OnProgramme.Last().ExpectedEndDate = expectedEndDate;

        var sut = new UpdateLearningRequestBodyBuilder(
            Mock.Of<ILearningSupportService>(),
            Mock.Of<IBreaksInLearningService>(),
            Mock.Of<ICostsService>());

        // Act
        var actualRequestBody = sut.Build(command.Ukprn, command.UpdateLearnerRequest);

        // Assert
        actualRequestBody.OnProgramme.ExpectedEndDate.Should().Be(expectedEndDate);
    }

    [Test]
    public void Build_Sets_AchievementDate_From_LatestOnProgramme()
    {
        var fixture = new Fixture();

        // Arrange
        var command = BreaksInLearningTestHelper.CreateLearnerWithBreaksInLearning(false);
        var achievementDate = fixture.Create<DateTime>();
        command.UpdateLearnerRequest.Delivery.OnProgramme.Last().AchievementDate = achievementDate;

        var sut = new UpdateLearningRequestBodyBuilder(
            Mock.Of<ILearningSupportService>(),
            Mock.Of<IBreaksInLearningService>(),
            Mock.Of<ICostsService>());

        // Act
        var actualRequestBody = sut.Build(command.Ukprn, command.UpdateLearnerRequest);

        // Assert
        actualRequestBody.OnProgramme.AchievementDate.Should().Be(achievementDate);
    }

    [Test]
    public void Build_Sets_PauseDate_From_LatestOnProgramme()
    {
        var fixture = new Fixture();

        // Arrange
        var command = BreaksInLearningTestHelper.CreateLearnerWithBreaksInLearning(false);
        var pauseDate = fixture.Create<DateTime>();
        command.UpdateLearnerRequest.Delivery.OnProgramme.Last().PauseDate = pauseDate;

        var sut = new UpdateLearningRequestBodyBuilder(
            Mock.Of<ILearningSupportService>(),
            Mock.Of<IBreaksInLearningService>(),
            Mock.Of<ICostsService>());

        // Act
        var actualRequestBody = sut.Build(command.Ukprn, command.UpdateLearnerRequest);

        // Assert
        actualRequestBody.OnProgramme.PauseDate.Should().Be(pauseDate);
    }

    [Test]
    public void Build_Sets_CareDetails_From_LatestOnProgramme()
    {
        var fixture = new Fixture();

        // Arrange
        var command = BreaksInLearningTestHelper.CreateLearnerWithBreaksInLearning(false);
        var careDetails = fixture.Create<Requests.Care>();
        command.UpdateLearnerRequest.Delivery.OnProgramme.Last().Care = careDetails;

        var sut = new UpdateLearningRequestBodyBuilder(
            Mock.Of<ILearningSupportService>(),
            Mock.Of<IBreaksInLearningService>(),
            Mock.Of<ICostsService>());

        // Act
        var actualRequestBody = sut.Build(command.Ukprn, command.UpdateLearnerRequest);

        // Assert
        actualRequestBody.Learner.Care.HasEHCP.Should().Be(command.UpdateLearnerRequest.Learner.HasEhcp);
        actualRequestBody.Learner.Care.IsCareLeaver.Should().Be(careDetails.Careleaver);
        actualRequestBody.Learner.Care.CareLeaverEmployerConsentGiven.Should().Be(careDetails.EmployerConsent);
    }

    [Test]
    public void Build_WithCreateLearnerRequest_MapsPropertiesCorrectly()
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

        var sut = new UpdateLearningRequestBodyBuilder(
            Mock.Of<ILearningSupportService>(),
            Mock.Of<IBreaksInLearningService>(),
            Mock.Of<ICostsService>());

        // Act
        var actualRequestBody = sut.Build(10005077, createRequest);

        // Assert
        actualRequestBody.Learner.FirstName.Should().Be("John");
        actualRequestBody.Learner.LastName.Should().Be("Doe");
        actualRequestBody.Learner.EmailAddress.Should().Be("john.doe@test.com");
        actualRequestBody.Learner.DateOfBirth.Should().Be(new DateTime(2000, 1, 1));
        actualRequestBody.Learner.Care.HasEHCP.Should().BeTrue();
        actualRequestBody.Learner.Care.IsCareLeaver.Should().BeTrue();
        actualRequestBody.Learner.Care.CareLeaverEmployerConsentGiven.Should().BeTrue();
    }
}
