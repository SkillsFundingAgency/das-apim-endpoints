using SFA.DAS.LearnerData.Api.AcceptanceTests.Models;
using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;
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
                    Instalments = GetEarningsInstalments(apprenticeshipModel),
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

        var sldLearnerData = new UpdateLearnerRequest
        {
            Learner = new LearnerRequestDetails
            {
                Uln = long.Parse(learning.Uln)
            },
            Delivery = new UpdateLearnerRequestDeliveryDetails
            {
                OnProgramme = GetSldOnProgrammes(apprenticeshipModel)
            }
        };

        return new InnerApiResponses
        {
            UnPagedLearningsInnerApiResponse = [learning],
            EarningsInnerApiResponse = new GetFm36DataResponse { Apprenticeships = new List<Apprenticeship> { earnings } },
            SldLearnerData = [sldLearnerData]
        };
    }

    /// <summary>
    /// Returns the test configured instalments, or if none are configured,
    /// then generates instalments that are close enough to be valid for test purposes
    /// </summary>
    /// <remarks>
    /// The generated instalments will not be correctly calculated, but will be close enough to ensure parsing code does not fall over
    /// For accurate instalments the Instalment models should be configured in the test data
    /// </remarks>
    private static List<Instalment> GetEarningsInstalments(ApprenticeshipModel apprenticeshipModel)
    {
        if(apprenticeshipModel.Instalments.Any())
        {
            return apprenticeshipModel.Instalments.Select(x => new SharedOuterApi.InnerApi.Responses.Earnings.Instalment
            {
                EpisodePriceKey = apprenticeshipModel.PriceEpisodes.Single(y => y.PriceEpisodeId == x.PriceEpisodeId).Key,
                AcademicYear = x.AcademicYear,
                DeliveryPeriod = x.DeliveryPeriod,
                Amount = x.Amount,
                InstalmentType = x.InstalmentType
            }).ToList();
        }

        var instalments = new List<Instalment>();
        foreach (var priceEpisode in apprenticeshipModel.PriceEpisodes)
        {
            var instalmentDate = priceEpisode.StartDate;
            var numberOfInstalments = priceEpisode.StartDate.GetNumberOfIncludedCensusDatesUntil(priceEpisode.EndDate);
            
            if(numberOfInstalments == 0 && priceEpisode.StartDate < priceEpisode.EndDate)
                numberOfInstalments = 1; // Ensure at least one instalment if episode has duration but is shorter than a month


            for (var i = 0; i < numberOfInstalments; i++)
            {
                instalments.Add(new Instalment
                {
                    EpisodePriceKey = priceEpisode.Key,
                    AcademicYear = instalmentDate.ToAcademicYear(),
                    DeliveryPeriod = instalmentDate.ToDeliveryPeriod(),
                    Amount = 100,
                    InstalmentType = "Regular"
                });
                instalmentDate = instalmentDate.AddMonths(1);
            }
        }

        return instalments;
    }

    private static List<OnProgrammeRequestDetails> GetSldOnProgrammes(ApprenticeshipModel apprenticeshipModel)
    {
        if(!apprenticeshipModel.LearningDeliveries.Any())
        {
            return new List<OnProgrammeRequestDetails>
            {
                new OnProgrammeRequestDetails
                {
                    AimSequenceNumber = 1,
                    StartDate = apprenticeshipModel.PriceEpisodes.Min(x => x.StartDate),
                    ExpectedEndDate = apprenticeshipModel.PriceEpisodes.Max(x => x.EndDate),
                    LearnAimRef = "DefaultProg"
                }
            };
        }

        return apprenticeshipModel.LearningDeliveries.Select(ld => new OnProgrammeRequestDetails
        {
            StartDate = ld.StartDate,
            ExpectedEndDate = ld.ExpectedEndDate,
            PauseDate = ld.ActualEndDate,
            AimSequenceNumber = ld.AimSequenceNumber,
            LearnAimRef = ld.LearnAimRef
        }).ToList();
    }
}
