using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;

namespace SFA.DAS.LearnerData.TestHelpers;

public static class LearnerBuilder
{
    // Concrete implementation to allow usage of the builder methods.
    // we can use this because the data being generated is for fm36 tests only.
    // these concrete class have their own tests to validate their behaviour.
    private static UpdateLearningPutRequestBuilder _updateLearningPutRequestBuilder = 
        new UpdateLearningPutRequestBuilder(new LearningSupportService(), new BreaksInLearningService(), new CostsService());

    internal static Learning BuildLearningInnerApiResponse(TestLearner testLearner)
    {
        var sldLearner = testLearner.UpdateLearnerRequest.Learner;
        var sldDelivery = testLearner.UpdateLearnerRequest.Delivery;

        var firstOnProg = sldDelivery.OnProgramme.OrderBy(x => x.StartDate).First();
        var latestOnProg = sldDelivery.OnProgramme.OrderByDescending(x => x.StartDate).First();

        var command = new UpdateLearnerCommand
        {
            LearningKey = testLearner.LearningKey,
            Ukprn = testLearner.Ukprn,
            UpdateLearnerRequest = testLearner.UpdateLearnerRequest
        };

        var updateLearningRequest = _updateLearningPutRequestBuilder.Build(command);

        return new Learning
        {
            Key = command.LearningKey,
            Uln = sldLearner.Uln.ToString(),
            StartDate = sldDelivery.OnProgramme.Min(x => x.StartDate),
            PlannedEndDate = sldDelivery.OnProgramme.Max(x => x.ExpectedEndDate),
            WithdrawnDate = latestOnProg.WithdrawalDate,
            AgeAtStartOfApprenticeship = (int)((firstOnProg.StartDate - sldLearner.Dob).TotalDays / 365),
            Episodes = ExtractLearningInnerEpisodes(testLearner, updateLearningRequest)
        };
    }

    internal static Apprenticeship BuildEarningsInnerApiResponse(TestLearner testLearner, Learning learningInnerRecord)
    {
        var apprenticeship = new Apprenticeship
        {
            Ukprn = testLearner.Ukprn,
            Key = testLearner.LearningKey,
            FundingLineType = testLearner.FundingLineType,
            Episodes = ExtractEarningInnnerEpisodes(testLearner, learningInnerRecord)
        };

        return apprenticeship;
    }

    /// <summary>
    /// Currently only returns one episode as we have no functional code which would create multiple episodes yet.
    /// </summary>
    private static List<SharedOuterApi.InnerApi.Responses.Learning.Episode> ExtractLearningInnerEpisodes(TestLearner testLearner, UpdateLearningApiPutRequest updateLearningRequest)
    {
        var prices = new List<SharedOuterApi.InnerApi.Responses.Learning.EpisodePrice>();

        var lastOnProg = testLearner.UpdateLearnerRequest.Delivery.OnProgramme
            .OrderByDescending(x => x.StartDate)
            .First();

        var endDate = lastOnProg.ExpectedEndDate;

        // start with the last cost and work backwards using the FromDate (startDate) as the end date for the next price
        foreach (var cost in updateLearningRequest.Data.OnProgramme.Costs.OrderByDescending(x => x.FromDate))
        {
            var totalPrice = cost.TrainingPrice + (cost.EpaoPrice ?? 0);

            prices.Add(new SharedOuterApi.InnerApi.Responses.Learning.EpisodePrice
            {
                Key = Guid.NewGuid(),
                StartDate = cost.FromDate,
                EndDate = endDate,
                TrainingPrice = cost.TrainingPrice,
                EndPointAssessmentPrice = cost.EpaoPrice,
                FundingBandMaximum = testLearner.FundingBandMax,
                TotalPrice = totalPrice
            });

            endDate = cost.FromDate.AddDays(-1);
        }


        var episode = new SharedOuterApi.InnerApi.Responses.Learning.Episode
        {
            Key = Guid.NewGuid(),
            TrainingCode = testLearner.TrainingCode,
            LastDayOfLearning = testLearner.UpdateLearnerRequest.Delivery.OnProgramme.Max(x => x.WithdrawalDate),
            Prices = prices.OrderBy(x => x.StartDate).ToList()
        };


        return new List<SharedOuterApi.InnerApi.Responses.Learning.Episode> { episode };
    }

    /// <summary>
    /// Currently only returns one episode as we have no functional code which would create multiple episodes yet.
    /// </summary>
    private static List<SharedOuterApi.InnerApi.Responses.Earnings.Episode> ExtractEarningInnnerEpisodes(TestLearner testLearner, Learning learningInnerRecord)
    {
        var learningEpisode = learningInnerRecord.Episodes.First();
        var totalPrice = learningEpisode.Prices.OrderBy(x => x.StartDate).Last().TotalPrice;

        var episode = learningInnerRecord.Episodes.Select(e => new SharedOuterApi.InnerApi.Responses.Earnings.Episode
        {
            Key = e.Key,
            NumberOfInstalments = 0,
            Instalments = new List<Instalment>(),
            AdditionalPayments = new List<AdditionalPayment>(),
            CompletionPayment = totalPrice * 0.2m,
            OnProgramTotal = totalPrice * 0.8m
        }).First();

        if (testLearner.Instalments != null && testLearner.Instalments.Any())
        {
            episode.Instalments = testLearner.Instalments;
            episode.NumberOfInstalments = testLearner.Instalments.Count;
        }
        else
        {
            episode.Instalments = GenerateInstalments(testLearner, learningEpisode);
            episode.NumberOfInstalments = episode.Instalments.Count;
        }

        episode.AdditionalPayments = testLearner.AdditionalPayments ?? new List<AdditionalPayment>();

        return new List<SharedOuterApi.InnerApi.Responses.Earnings.Episode> { episode };
    }

    /// <summary>
    /// This is a basic implementation to generate some instalments. It does not have the full complexities
    /// of the earnings service implementation. Its purpose is for general validation. For more complex test scenarios
    /// expected instalments should be provided in the TestLearner.
    /// </summary>
    private static List<Instalment> GenerateInstalments(TestLearner testLearner, SharedOuterApi.InnerApi.Responses.Learning.Episode learningEpisode)
    {
        var instalments = new List<Instalment>();

        foreach (var program in testLearner.UpdateLearnerRequest.Delivery.OnProgramme)
        {
            var periodInLearningKey = Guid.NewGuid();
            var programEndDate = (new []{program.WithdrawalDate, program.ExpectedEndDate, program.ExpectedEndDate}).Min();
            var numberOfCosts = program.Costs?.Count ?? 0;

            for (var costIndex = 0; costIndex < numberOfCosts; costIndex++)
            {
                var cost = program.Costs![costIndex];
                var startDate = cost.FromDate!.Value;
                var endDate = programEndDate;

                if ((costIndex + 1) < numberOfCosts)
                {
                    // not the last cost, so end date is day before next cost start date
                    endDate = program.Costs[costIndex + 1].FromDate!.Value.AddDays(-1); 
                }

                var matchingPrice = learningEpisode.Prices.First(p=> 
                    p.StartDate == cost.FromDate && p.TrainingPrice == cost.TrainingPrice && p.EndPointAssessmentPrice == cost.EpaoPrice);
            
                var numberOfInstalments = startDate.GetNumberOfIncludedCensusDatesUntil(endDate!.Value);
                var instalmentPrice = matchingPrice.TotalPrice / numberOfInstalments;

                for(var i = 0; i < numberOfInstalments; i++)
                {
                    var censusDate = startDate.AddMonths(i);

                    var instalment = new Instalment
                    {
                        AcademicYear = censusDate.ToAcademicYear(),
                        DeliveryPeriod = censusDate.ToDeliveryPeriod(),
                        Amount = instalmentPrice,
                        EpisodePriceKey = matchingPrice.Key,
                        PeriodInLearningKey = periodInLearningKey,
                        InstalmentType = "Regular"
                    };

                    instalments.Add(instalment);
                }
            }

        }

        return instalments;
    }
}
