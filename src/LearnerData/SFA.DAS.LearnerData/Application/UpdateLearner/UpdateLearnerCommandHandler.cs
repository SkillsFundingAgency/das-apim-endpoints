using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.Application.UpdateLearner;

public class UpdateLearnerCommandHandler(
    ILogger<UpdateLearnerCommandHandler> logger,
    ILearningApiClient<LearningApiConfiguration> learningApiClient,
    IEarningsApiClient<EarningsApiConfiguration> earningsApiClient,
    ILearningSupportService learningSupportService
    ) : IRequestHandler<UpdateLearnerCommand>
{
    public async Task Handle(UpdateLearnerCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating learner with key {LearningKey}", command.LearningKey);
        var request = CreateUpdateLearnerApiPutRequest(command.LearningKey, command);

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
                    await earningsApiClient.UpdateCompletionDate(command, logger);
                    break;
                case UpdateLearnerApiPutResponse.LearningUpdateChanges.MathsAndEnglish:
                    await earningsApiClient.UpdateMathAndEnglish(command, logger);
                    break;
                case UpdateLearnerApiPutResponse.LearningUpdateChanges.LearningSupport:
                    await earningsApiClient.UpdateLearningSupport(command, updateLearningApiPutRequest, logger);
                    break;
                case UpdateLearnerApiPutResponse.LearningUpdateChanges.Prices:
                case UpdateLearnerApiPutResponse.LearningUpdateChanges.ExpectedEndDate:
                    updatePrices = true;
                    break;
                case UpdateLearnerApiPutResponse.LearningUpdateChanges.Withdrawal:
                    await earningsApiClient.WithdrawLearner(command, logger);
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
            await earningsApiClient.UpdatePrices(command.LearningKey, updateLearningApiPutResponse, logger);
        }
    }

    private UpdateLearningApiPutRequest CreateUpdateLearnerApiPutRequest(Guid learnerKey, UpdateLearnerCommand command)
    {
        /*
       Compare each OnProgramme element with the previous element to ensure they are for the same “episode” (i.e. standardCode and agreementIdare the same (Ukprn is implied).
        Any items in the array which aren’t for the same episode as they’ll be handled in later features.
       Add a new BreakInLearning item to the BreakInLearning array with the start date of the end of the previous episode (confirm field) and with an end date of the start of the next episode.
       Merge together the costs arrays from both OnProgramme elements, removing the break if the costs are the same before and after the break.
       Merge together the values from the correct OnProgramme elements to create the required values for the inner:
       WithdrawalDate - last element
       ExpectedEndDate - last element
         */

        //get the first element in the OnProg - this is the "driver"
        //get all other on-progs with same StandardCode and AgreementId (ignore all others)
        //calculate the breaks in learning
        //merge costs array (delete if same before and after break)
        //update on prog with fields from last onProg where necessary
        // -- eg. WithdrawalDate, ExpectedEndDate, PauseDate (?)

        var orderedOnProgs = command.UpdateLearnerRequest.Delivery.OnProgramme
            .OrderBy(x => x.StartDate); //todo: check order

        var primaryOnProg = orderedOnProgs.First();
        var onProgs = orderedOnProgs
                .Where(x => x.StandardCode == primaryOnProg.StandardCode && x.AgreementId == primaryOnProg.AgreementId)
                .ToList();

        var lastOnProg = orderedOnProgs.Last();

        var breaksInLearning = CalculateBreaksInLearning(onProgs);

        var learningSupport = learningSupportService.GetCombinedLearningSupport(onProgs,
            command.UpdateLearnerRequest.Delivery.EnglishAndMaths, breaksInLearning.Breaks);

        var body = new UpdateLearningRequestBody
        {
            Delivery = new Delivery
            {
                WithdrawalDate = lastOnProg.WithdrawalDate
            },
            Learner = new LearningUpdateDetails
            {
                FirstName = command.UpdateLearnerRequest.Learner.FirstName,
                LastName = command.UpdateLearnerRequest.Learner.LastName,
                EmailAddress = command.UpdateLearnerRequest.Learner.Email,
                CompletionDate = lastOnProg.CompletionDate
            },
            OnProgramme = new OnProgrammeDetails
            {
                ExpectedEndDate = lastOnProg.ExpectedEndDate,
                Costs = breaksInLearning.Costs.GetCostsOrDefault(primaryOnProg.StartDate),
                PauseDate = lastOnProg.PauseDate,
                BreaksInLearning = breaksInLearning.Breaks
            },
            MathsAndEnglishCourses = command.UpdateLearnerRequest.Delivery.EnglishAndMaths.Select(x =>
                new MathsAndEnglishDetails
                {
                    Amount = x.Amount,
                    CompletionDate = x.CompletionDate,
                    Course = x.Course,
                    PlannedEndDate = x.EndDate,
                    PriorLearningPercentage = x.PriorLearningPercentage,
                    StartDate = x.StartDate,
                    WithdrawalDate = x.WithdrawalDate
                }).ToList(),
            LearningSupport = learningSupport
        };

        return new UpdateLearningApiPutRequest(learnerKey, body);
    }

    private static (List<BreakInLearning> Breaks, List<CostDetails> Costs)
        CalculateBreaksInLearning(List<OnProgrammeRequestDetails> onProgrammeItems)
    {
        var breaks = new List<BreakInLearning>();
        var mergedCosts = new List<CostDetails>();

        for (var i = 0; i < onProgrammeItems.Count; i++)
        {
            var current = onProgrammeItems[i];

            // Merge costs: union all, but discard redundant consecutive entries
            if (current.Costs != null)
            {
                foreach (var cost in current.Costs)
                {
                    if (mergedCosts.Count == 0)
                    {
                        mergedCosts.Add(cost);
                    }
                    else
                    {
                        var last = mergedCosts.Last();
                        if (last.TrainingPrice == cost.TrainingPrice &&
                            last.EpaoPrice == cost.EpaoPrice)
                        {
                            // Identical apart from FromDate → carry forward the first FromDate
                            // Do nothing (keep last as-is)
                        }
                        else
                        {
                            mergedCosts.Add(cost);
                        }
                    }
                }
            }

            // Breaks in learning (skip last item check)
            if (i < onProgrammeItems.Count - 1)
            {
                var next = onProgrammeItems[i + 1];

                if (current.ActualEndDate.HasValue && current.ActualEndDate < next.StartDate)
                {
                    var gapStart = current.ActualEndDate.Value.AddDays(1);
                    var gapEnd = next.StartDate.AddDays(-1);

                    breaks.Add(new BreakInLearning
                    {
                        StartDate = gapStart,
                        EndDate = gapEnd
                    });
                }
            }
        }

        return (breaks, mergedCosts);
    }
}