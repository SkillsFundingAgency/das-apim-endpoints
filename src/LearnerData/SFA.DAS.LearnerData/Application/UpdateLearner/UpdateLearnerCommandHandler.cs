using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerData.Services.SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.Application.UpdateLearner;

public class UpdateLearnerCommandHandler(
    ILogger<UpdateLearnerCommandHandler> logger,
    ILearningApiClient<LearningApiConfiguration> learningApiClient,
    IEarningsApiClient<EarningsApiConfiguration> earningsApiClient,
    IUpdateLearningPutRequestBuilder updateLearningPutRequestBuilder,
	ICoursesApiClient<CoursesApiConfiguration> coursesApiClient
    ) : IRequestHandler<UpdateLearnerCommand>
{
    public async Task Handle(UpdateLearnerCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating learner with key {LearningKey}", command.LearningKey);
        var request = updateLearningPutRequestBuilder.Build(command);

        var learningResponse = await learningApiClient.PutWithResponseCode<UpdateLearningRequestBody, UpdateLearnerApiPutResponse>(request);

        if (!learningResponse.StatusCode.IsSuccessStatusCode())
        {
            logger.LogError("Failed to update learner with key {LearningKey}. Status code: {StatusCode}",
                command.LearningKey, learningResponse.StatusCode);
            throw new Exception($"Failed to update learner with key {command.LearningKey}. Status code: {learningResponse.StatusCode}.");
        }

        var learningApiPutResponse = learningResponse.Body;
        if (!learningApiPutResponse.Changes.Any())
        {
            logger.LogInformation("No changes detected for learner with key {LearningKey}", command.LearningKey);
            return;
        }

        logger.LogInformation("Learner with key {LearningKey} updated successfully. Changes: {@Changes}",
            command.LearningKey, string.Join(", ", learningApiPutResponse));

        await UpdateEarnings(command, request, learningApiPutResponse);

        logger.LogInformation("Earnings updated for learner with key {LearningKey}", command.LearningKey);
    }

    private async Task UpdateEarnings(UpdateLearnerCommand command, UpdateLearningApiPutRequest updateLearningApiPutRequest, UpdateLearnerApiPutResponse updateLearningApiPutResponse)
    {
        var updatePrices = false;

        foreach (var change in updateLearningApiPutResponse.Changes)
        {
            switch (change)
            {
                case UpdateLearnerApiPutResponse.LearningUpdateChanges.CompletionDate:
                    await earningsApiClient.UpdateCompletionDate(command, updateLearningApiPutRequest, logger);
                    break;
                case UpdateLearnerApiPutResponse.LearningUpdateChanges.MathsAndEnglish:
                    await earningsApiClient.UpdateMathAndEnglish(command, updateLearningApiPutRequest, logger);
                    break;
                case UpdateLearnerApiPutResponse.LearningUpdateChanges.LearningSupport:
                    await earningsApiClient.UpdateLearningSupport(command, updateLearningApiPutRequest, logger);
                    break;
                case UpdateLearnerApiPutResponse.LearningUpdateChanges.Prices:
                case UpdateLearnerApiPutResponse.LearningUpdateChanges.ExpectedEndDate:
                    updatePrices = true;
                    break;
                case UpdateLearnerApiPutResponse.LearningUpdateChanges.Withdrawal:
                    await earningsApiClient.WithdrawLearner(command, updateLearningApiPutRequest, logger);
                    break;
                case UpdateLearnerApiPutResponse.LearningUpdateChanges.ReverseWithdrawal:
                    await earningsApiClient.ReverseWithdrawal(command, logger);
                    break;
                case UpdateLearnerApiPutResponse.LearningUpdateChanges.BreakInLearningRemoved:
                    await earningsApiClient.RemoveBreakInLearning(command, logger);
                    break;
                case UpdateLearnerApiPutResponse.LearningUpdateChanges.BreakInLearningStarted:
                    await earningsApiClient.StartBreakInLearning(command, updateLearningApiPutRequest, logger);
                    break;
                case UpdateLearnerApiPutResponse.LearningUpdateChanges.BreaksInLearningUpdated:
                    await earningsApiClient.UpdateBreaksInLearning(command, updateLearningApiPutRequest, updateLearningApiPutResponse, logger);
                    break;
            }
        }

        if (updatePrices)
        {
            var fundingBandMaximum = await GetFundingBandMaximum(command);
            await earningsApiClient.UpdatePrices(command.LearningKey, updateLearningApiPutResponse, fundingBandMaximum, logger);
        }
    }

    private async Task<int> GetFundingBandMaximum(UpdateLearnerCommand command)
    {
        var onProgramme = command.UpdateLearnerRequest.Delivery.OnProgramme.First();
        var standardId = onProgramme.StandardCode.ToString();
        var startDate = onProgramme.StartDate;

        var response = await coursesApiClient.Get<StandardDetailResponse>(new GetStandardDetailsByIdRequest(standardId));

        return response.MaxFundingOn(startDate);
    }
}