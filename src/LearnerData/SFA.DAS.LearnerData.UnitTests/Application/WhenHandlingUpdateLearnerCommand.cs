using AutoFixture.NUnit3;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LearnerData.Application;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.LearnerData.UnitTests.Application;

public class WhenHandlingUpdateLearnerCommand
{
    [Test, MoqAutoData]
    public async Task Then_Learner_Is_Updated_Successfully_With_Changes(
        Guid learningKey,
        DateTime completionDate,
        List<MathsAndEnglish> mathsAndEnglishCourses,
        [Frozen] Mock<ILearningApiClient<LearningApiConfiguration>> learningApiClient,
        [Frozen] Mock<IEarningsApiClient<EarningsApiConfiguration>> earningsApiClient,
        [Frozen] Mock<ILogger<UpdateLearnerCommandHandler>> logger,
        UpdateLearnerCommandHandler handler)
    {
        // Arrange
        var command = CreateCommand(learningKey, completionDate, mathsAndEnglishCourses);

        MockLearningApiResponse(learningApiClient, new UpdateLearnerApiPutResponse { LearningUpdateChanges.CompletionDate }, HttpStatusCode.OK);

        earningsApiClient.Setup(x => x.Patch(It.IsAny<SaveCompletionApiPutRequest>()))
            .Returns(Task.CompletedTask);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        learningApiClient.Verify(x =>
            x.PutWithResponseCode<UpdateLearningRequestBody, UpdateLearnerApiPutResponse>(
                It.Is<UpdateLearningApiPutRequest>(r => r.Data.Learner.CompletionDate == completionDate)), Times.Once);

        earningsApiClient.Verify(x => x.Patch(It.Is<SaveCompletionApiPutRequest>(
            r => r.Data.CompletionDate == completionDate)), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_No_Earnings_Updated_If_No_Changes(
        Guid learningKey,
        DateTime completionDate,
        List<MathsAndEnglish> mathsAndEnglishCourses,
        [Frozen] Mock<ILearningApiClient<LearningApiConfiguration>> learningApiClient,
        [Frozen] Mock<IEarningsApiClient<EarningsApiConfiguration>> earningsApiClient,
        [Frozen] Mock<ILogger<UpdateLearnerCommandHandler>> logger,
        UpdateLearnerCommandHandler handler)
    {
        // Arrange
        var command = CreateCommand(learningKey, completionDate, mathsAndEnglishCourses);

        MockLearningApiResponse(learningApiClient, new UpdateLearnerApiPutResponse(), HttpStatusCode.OK);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        earningsApiClient.Verify(x => x.Patch(It.IsAny<SaveCompletionApiPutRequest>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Then_Logs_Error_If_Learner_Update_Fails(
        Guid learningKey,
        DateTime completionDate,
        List<MathsAndEnglish> mathsAndEnglishCourses,
        [Frozen] Mock<ILearningApiClient<LearningApiConfiguration>> learningApiClient,
        [Frozen] Mock<ILogger<UpdateLearnerCommandHandler>> logger,
        UpdateLearnerCommandHandler handler)
    {
        // Arrange
        var command = CreateCommand(learningKey, completionDate, mathsAndEnglishCourses);

        MockLearningApiResponse(learningApiClient, new UpdateLearnerApiPutResponse(), HttpStatusCode.InternalServerError, "error");

        // Act/Assert
        Assert.ThrowsAsync<Exception>(async()=> await handler.Handle(command, CancellationToken.None));

    }

    [Test, MoqAutoData]
    public async Task Then_Learner_Is_Updated_Successfully_With_MathsAndEnglish_Changes(
        Guid learningKey,
        DateTime completionDate,
        List<MathsAndEnglish> mathsAndEnglishCourses,
        [Frozen] Mock<ILearningApiClient<LearningApiConfiguration>> learningApiClient,
        [Frozen] Mock<IEarningsApiClient<EarningsApiConfiguration>> earningsApiClient,
        [Frozen] Mock<ILogger<UpdateLearnerCommandHandler>> logger,
        UpdateLearnerCommandHandler handler)
    {
        // Arrange
        var command = CreateCommand(learningKey, completionDate, mathsAndEnglishCourses);

        MockLearningApiResponse(learningApiClient, new UpdateLearnerApiPutResponse { LearningUpdateChanges.MathsAndEnglish }, HttpStatusCode.OK);

        earningsApiClient.Setup(x => x.Patch(It.IsAny<SaveCompletionApiPutRequest>()))
            .Returns(Task.CompletedTask);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        learningApiClient.Verify(x =>
            x.PutWithResponseCode<UpdateLearningRequestBody, UpdateLearnerApiPutResponse>(
                It.Is<UpdateLearningApiPutRequest>(r => r.Data.Learner.CompletionDate == completionDate)), Times.Once);

        earningsApiClient.Verify(x => x.Patch(It.Is<SaveMathsAndEnglishApiPatchRequest>(
            r => Matches(r.Data, mathsAndEnglishCourses))), Times.Once);
    }

    private static UpdateLearnerCommand CreateCommand(Guid learningKey, DateTime completionDate, List<MathsAndEnglish> mathsAndEnglishCourses)
    {
        return new UpdateLearnerCommand
        {
            LearningKey = learningKey,
            UpdateLearnerRequest = new UpdateLearnerRequest
            {
                Delivery = new UpdateLearnerRequestDeliveryDetails
                {
                    CompletionDate = completionDate,
                    MathsAndEnglishCourses = mathsAndEnglishCourses
                }
            }
        };
    }

    private static void MockLearningApiResponse(
        Mock<ILearningApiClient<LearningApiConfiguration>> learningApiClient,
        UpdateLearnerApiPutResponse responseBody,
        HttpStatusCode statusCode,
        string errorContent = "")
    {
        var response = new ApiResponse<UpdateLearnerApiPutResponse>(
            responseBody,
            statusCode,
            errorContent);

        learningApiClient.Setup(x => 
            x.PutWithResponseCode<UpdateLearningRequestBody, UpdateLearnerApiPutResponse>(It.IsAny<UpdateLearningApiPutRequest>()))
        .ReturnsAsync(response);
    }

    private static bool Matches(SaveMathsAndEnglishRequest request, List<MathsAndEnglish> courses)
    {
        return request.Count == courses.Count &&
               request.All(r => courses.Any(c => c.StartDate == r.StartDate &&
                                                 c.PlannedEndDate == r.EndDate &&
                                                 c.Course == r.Course &&
                                                 c.Amount == r.Amount &&
                                                 c.WithdrawalDate == r.WithdrawalDate &&
                                                 c.PriorLearningPercentage == r.PriorLearningAdjustmentPercentage &&
                                                 c.CompletionDate == r.ActualEndDate));
    }
}
