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
                //LearningDeliveries = 
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