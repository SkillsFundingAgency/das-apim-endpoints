using SFA.DAS.LearnerData.Api.AcceptanceTests.Models;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using Episode = SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning.Episode;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Extensions;

public static class ApprenticeshipModelExtensions
{
    public static InnerApiResponses GetInnerApiResponses(this ApprenticeshipModel apprenticeshipModel)
    {
        var learning = new Learning
        {
            Key = Guid.NewGuid(),
            AgeAtStartOfApprenticeship = 18,
            StartDate = apprenticeshipModel.PriceEpisodes.Min(x => x.StartDate),
            PlannedEndDate = apprenticeshipModel.PriceEpisodes.Max(x => x.EndDate),
            WithdrawnDate = apprenticeshipModel.WithdrawnDate,
            Uln = "123567",
            Episodes =
            [
                new Episode
                {
                    Key = Guid.NewGuid(),
                    TrainingCode = "test",
                    Prices = apprenticeshipModel.PriceEpisodes.Select(x => new EpisodePrice
                    {
                        Key = x.Key,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        EndPointAssessmentPrice = 1000,
                        FundingBandMaximum = 10000,
                        TotalPrice = 11000,
                        TrainingPrice = 10000
                    }).ToList()
                }
            ]
        };

        var earnings = new SharedOuterApi.InnerApi.Responses.Earnings.Apprenticeship
        {
            Key = learning.Key,
            Episodes = new List<SharedOuterApi.InnerApi.Responses.Earnings.Episode>()
            {
                new SharedOuterApi.InnerApi.Responses.Earnings.Episode
                {
                    Key = Guid.NewGuid(),
                    Instalments = apprenticeshipModel.Instalments.Select(x => new SharedOuterApi.InnerApi.Responses.Earnings.Instalment
                    {
                        EpisodePriceKey = apprenticeshipModel.PriceEpisodes.Single(y => y.PriceEpisodeId == x.PriceEpisodeId).Key,
                        AcademicYear = x.AcademicYear,
                        DeliveryPeriod = x.DeliveryPeriod,
                        Amount = x.Amount,
                        InstalmentType = x.InstalmentType
                    }).ToList(),
                    AdditionalPayments = apprenticeshipModel.AdditionalPayments.Select(x =>
                        new SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings.AdditionalPayment
                        {
                            AdditionalPaymentType = x.Type,
                            Amount = x.Amount,
                            AcademicYear = x.AcademicYear,
                            DueDate = x.DueDate,
                            DeliveryPeriod = x.DeliveryPeriod
                        }).ToList()
                }
            },
            FundingLineType = "test",
            Ukprn = 10005077
        };

        //For readability, tests need not describe instalments where they are not relevant to the test
        //However, FM36 block generation requires each price episode to have at least one instalment in order to join episodes
        //So, we can auto-generate if instalment information is missing
        foreach (var episode in earnings.Episodes)
        {
            if (!episode.Instalments.Any())
            {
                foreach (var price in learning.Episodes.SelectMany(apprenticeshipEpisode => apprenticeshipEpisode.Prices))
                {
                    episode.Instalments.Add(new Instalment
                    {
                        Amount = 0,
                        AcademicYear = 0,
                        DeliveryPeriod = 0,
                        EpisodePriceKey = price.Key,
                        InstalmentType = "Regular"
                    });
                }
            }
        }

        return new InnerApiResponses
        {
            UnPagedLearningsInnerApiResponse = [learning],
            EarningsInnerApiResponses = new List<GetFm36DataResponse> { new GetFm36DataResponse { Apprenticeship = earnings } }
        };
    }
}
