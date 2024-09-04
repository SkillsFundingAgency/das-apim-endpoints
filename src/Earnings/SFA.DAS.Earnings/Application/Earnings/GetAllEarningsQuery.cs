using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using SFA.DAS.SharedOuterApi.Interfaces;


namespace SFA.DAS.Earnings.Application.Earnings;

public class GetAllEarningsQuery : IRequest<GetAllEarningsQueryResult>
{
    public long Ukprn { get; set; }
}

public class GetAllEarningsQueryResult
{
    public FM36Learner[] FM36Learners { get; set; }
}

public class GetAllEarningsQueryHandler : IRequestHandler<GetAllEarningsQuery, GetAllEarningsQueryResult>
{
    private readonly IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration> _apprenticeshipsApiClient;
    private readonly IEarningsApiClient<EarningsApiConfiguration> _earningsApiClient;
    private readonly ICollectionCalendarApiClient<CollectionCalendarApiConfiguration> _collectionCalendarApiClient;

    public GetAllEarningsQueryHandler(IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration> apprenticeshipsApiClient,
        IEarningsApiClient<EarningsApiConfiguration> earningsApiClient,
        ICollectionCalendarApiClient<CollectionCalendarApiConfiguration> collectionCalendarApiClient)
    {
        _apprenticeshipsApiClient = apprenticeshipsApiClient;
        _earningsApiClient = earningsApiClient;
        _collectionCalendarApiClient = collectionCalendarApiClient;
    }

    public async Task<GetAllEarningsQueryResult> Handle(GetAllEarningsQuery request, CancellationToken cancellationToken)
    {
        var apprenticeshipInnerModel = await _apprenticeshipsApiClient.Get<GetApprenticeshipsResponse>(new GetApprenticeshipsRequest { Ukprn = request.Ukprn });
        var earningsInnerModel = await _earningsApiClient.Get<GetFm36DataResponse>(new GetFm36DataRequest(request.Ukprn));
        var currentAcademicYear = await _collectionCalendarApiClient.Get<GetAcademicYearsResponse>(new GetAcademicYearsRequest(DateTime.Now)); //todo will we want to be able to time travel in test scenarios here?

        //todo this line just to enable testing prior to refactor of earnings inner api/removal of durable entities
        for (int i = 0; i < earningsInnerModel.Count; i++) earningsInnerModel[i].Key = apprenticeshipInnerModel.Apprenticeships[i].Key;
        
        var result = new GetAllEarningsQueryResult
        {
            FM36Learners = apprenticeshipInnerModel.Apprenticeships.Select(apprenticeship => new FM36Learner
            {
                ULN = long.Parse(apprenticeship.Uln),
                LearnRefNumber = EarningsFM36Constants.LearnRefNumber,
                PriceEpisodes = apprenticeship.Episodes.SelectMany(episode => episode.Prices, (episode, price) => new PriceEpisode
                {
                    PriceEpisodeIdentifier = $"25-{episode.TrainingCode}-{price.StartDate:dd/MM/yyyy}",
                    PriceEpisodeValues = new PriceEpisodeValues
                    {
                        EpisodeStartDate = price.StartDate
                    }
                }).ToList(),
                LearningDeliveries = new List<LearningDelivery>
                {
                    new LearningDelivery
                    {
                        AimSeqNumber = 1,
                        LearningDeliveryValues = new LearningDeliveryValues
                        {
                            ActualDaysIL = EarningsFM36Constants.ActualDaysIL,
                            AdjStartDate = apprenticeship.StartDate,
                            AgeAtProgStart = apprenticeship.AgeAtStartOfApprenticeship,
                            AppAdjLearnStartDate = apprenticeship.StartDate,
                            ApplicCompDate = EarningsFM36Constants.ApplicCompDate,
                            CombinedAdjProp = EarningsFM36Constants.CombinedAdjProp,
                            Completed = EarningsFM36Constants.Completed,
                            FundStart = EarningsFM36Constants.FundStart,
                            LDApplic1618FrameworkUpliftTotalActEarnings = EarningsFM36Constants.LDApplic1618FrameworkUpliftTotalActEarnings,
                            LearnAimRef = EarningsFM36Constants.LearnAimRef,
                            LearnStartDate = apprenticeship.StartDate,
                            LearnDel1618AtStart = apprenticeship.AgeAtStartOfApprenticeship < 19,
                            LearnDelAppAccDaysIL = ((apprenticeship.PlannedEndDate < currentAcademicYear.EndDate
                                ? apprenticeship.PlannedEndDate
                                : currentAcademicYear.EndDate) - apprenticeship.StartDate).Days,
                            LearnDelApplicDisadvAmount = EarningsFM36Constants.LearnDelApplicDisadvAmount,
                            LearnDelApplicEmp1618Incentive = EarningsFM36Constants.LearnDelApplicEmp1618Incentive,
                            LearnDelApplicProv1618FrameworkUplift = EarningsFM36Constants.LearnDelApplicProv1618FrameworkUplift,
                            LearnDelApplicProv1618Incentive = EarningsFM36Constants.LearnDelApplicProv1618Incentive,
                            LearnDelAppPrevAccDaysIL = 
                                ((apprenticeship.PlannedEndDate < currentAcademicYear.EndDate
                                    ? apprenticeship.PlannedEndDate
                                    : currentAcademicYear.EndDate)
                                - (apprenticeship.StartDate > currentAcademicYear.StartDate
                                    ? apprenticeship.StartDate
                                    : currentAcademicYear.StartDate)).Days,
                            LearnDelDisadAmount = EarningsFM36Constants.LearnDelDisadAmount,
                            LearnDelEligDisadvPayment = EarningsFM36Constants.LearnDelEligDisadvPayment,
                            LearnDelEmpIdFirstAdditionalPaymentThreshold = EarningsFM36Constants.LearnDelEmpIdFirstAdditionalPaymentThreshold,
                            LearnDelEmpIdSecondAdditionalPaymentThreshold = EarningsFM36Constants.LearnDelEmpIdSecondAdditionalPaymentThreshold,

                            //the below two values assume days in learning & amounts for previous academic years (any provider) are 0 for our beta learners as per FLP-862
                            //but this logic will need expanding in the future
                            LearnDelHistDaysThisApp = (DateTime.Now - (apprenticeship.StartDate > currentAcademicYear.StartDate //todo this will need updating if we want to time travel as above
                                ? apprenticeship.StartDate
                                : currentAcademicYear.StartDate)).Days,
                            //LearnDelHistProgEarnings = earningsInnerModel
                        }
                    }
                }
                //PriceEpisodes = apprenticeship.Episodes.Select(episode => new PriceEpisode
                //{
                //    PriceEpisodeIdentifier = $"25-{episode.TrainingCode}-{episode.Prices.Min(price => price.StartDate):dd/MM/yyyy}",
                //    PriceEpisodeValues = new PriceEpisodeValues
                //    {
                //        //EpisodeStartDate = episode.
                //    }
                //}).ToList()
            }).ToArray()
        };

        //foreach (var apprenticeship in apprenticeshipInnerModel.Apprenticeships)
        //{
        //    var learner = new FM36Learner
        //    {
        //        ULN = long.Parse(apprenticeship.Uln),
        //        LearnRefNumber = EarningsFM36Constants.LearnRefNumber,
        //        PriceEpisodes = new List<PriceEpisode>()
        //    };

        //    foreach (var apprenticeshipEpisodePrice in apprenticeship.Episodes.SelectMany(e => e.Prices, (episode, price) => new { episode, price }))
        //    {
        //        if(apprenticeshipEpisodePrice.price.StartDate)
        //    }
        //}

        return result;
    }
}