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
        [Frozen] Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>> learningApiClient,
        [Frozen] Mock<IEarningsApiClient<EarningsApiConfiguration>> earningsApiClient,
        [Frozen] Mock<ILogger<UpdateLearnerCommandHandler>> logger,
        UpdateLearnerCommandHandler handler)
    {
        // Arrange
        var command = CreateCommand(learningKey, completionDate);

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
        [Frozen] Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>> learningApiClient,
        [Frozen] Mock<IEarningsApiClient<EarningsApiConfiguration>> earningsApiClient,
        [Frozen] Mock<ILogger<UpdateLearnerCommandHandler>> logger,
        UpdateLearnerCommandHandler handler)
    {
        // Arrange
        var command = CreateCommand(learningKey, completionDate);

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
        [Frozen] Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>> learningApiClient,
        [Frozen] Mock<ILogger<UpdateLearnerCommandHandler>> logger,
        UpdateLearnerCommandHandler handler)
    {
        // Arrange
        var command = CreateCommand(learningKey, completionDate);

        MockLearningApiResponse(learningApiClient, new UpdateLearnerApiPutResponse(), HttpStatusCode.InternalServerError, "error");

        // Act/Assert
        Assert.ThrowsAsync<Exception>(async()=> await handler.Handle(command, CancellationToken.None));

    }

    private static UpdateLearnerCommand CreateCommand(Guid learningKey, DateTime completionDate)
    {
        return new UpdateLearnerCommand
        {
            LearningKey = learningKey,
            UpdateLearnerRequest = new UpdateLearnerRequest
            {
                Delivery = new UpdateLearnerRequestDeliveryDetails
                {
                    CompletionDate = completionDate
                }
            }
        };
    }

    private static void MockLearningApiResponse(
        Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>> learningApiClient,
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
}
