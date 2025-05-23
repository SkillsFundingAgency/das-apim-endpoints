using SFA.DAS.Earnings.Api.AcceptanceTests.Models;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;
using Apprenticeship = SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships.Apprenticeship;
using Episode = SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships.Episode;

namespace SFA.DAS.Earnings.Api.AcceptanceTests.Extensions
{
    public static class ApprenticeshipModelExtensions
    {
        public static InnerApiResponses GetInnerApiResponses(this ApprenticeshipModel apprenticeshipModel)
        {
            var apprenticeship = new Apprenticeship
            {
                Key = Guid.NewGuid(),
                AgeAtStartOfApprenticeship = 18,
                StartDate = apprenticeshipModel.PriceEpisodes.Min(x => x.StartDate),
                PlannedEndDate = apprenticeshipModel.PriceEpisodes.Max(x => x.EndDate),
                WithdrawnDate = null,
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
                Key = apprenticeship.Key,
                Episodes = new List<SharedOuterApi.InnerApi.Responses.Earnings.Episode>()
                {
                    new SharedOuterApi.InnerApi.Responses.Earnings.Episode
                    {
                        Instalments = apprenticeshipModel.Instalments.Select(x => new SharedOuterApi.InnerApi.Responses.Earnings.Instalment
                        {
                            EpisodePriceKey = apprenticeshipModel.PriceEpisodes.Single(y => y.PriceEpisodeId == x.PriceEpisodeId).Key,
                            AcademicYear = x.AcademicYear,
                            DeliveryPeriod = x.DeliveryPeriod,
                            Amount = x.Amount
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

            return new InnerApiResponses
            {
                ApprenticeshipsInnerApiResponse = new GetApprenticeshipsResponse
                {
                    Ukprn = 10005077,
                    Apprenticeships = [apprenticeship]
                },
                EarningsInnerApiResponse = [earnings]

            };
        }
    }
}
