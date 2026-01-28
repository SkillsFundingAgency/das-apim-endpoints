using SFA.DAS.LearnerData.Application.Fm36;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Models;

public class InnerApiResponses
{
    public List<GetPagedLearnersFromLearningInner> PagedLearningsInnerApiResponse { get; set; } = new();
    public List<Learning> UnPagedLearningsInnerApiResponse { get; set; } = new();
    public GetFm36DataResponse EarningsInnerApiResponse { get; set; } = new();
}

internal static class InnerApiResponsesExtensions
{
    internal static List<UpdateLearnerRequest> GetSldData(this InnerApiResponses responses)
    {
        return responses.EarningsInnerApiResponse.Apprenticeships
            .Select(x => ConstructSldLearnerData(x, responses.UnPagedLearningsInnerApiResponse))
            .ToList();
    }

    private static UpdateLearnerRequest ConstructSldLearnerData(Apprenticeship earningsData, List<Learning> learningResponses)
    {
        var learning = learningResponses.First(l => l.Key == earningsData.Key);

        return new UpdateLearnerRequest
        {
            Learner = new LearnerRequestDetails
            {
                Uln = long.Parse(learning.Uln)
                // other properties not needed at this time
            }//,
            //Delivery = new UpdateLearnerRequestDeliveryDetails
            //{
            //    OnProgramme = earningsData.Episodes.SelectMany(x => x.).Select(op => new OnProgrammeRequestDetails
            //    {
            //        AimSeqNumber = op.AimSeqNumber,
            //        LearnAimRef = op.LearnAimRef,
            //        StartDate = op.StartDate,
            //        PlannedEndDate = op.PlannedEndDate,
            //        ActualEndDate = op.ActualEndDate,
            //        PriceEpisodes = x.Episodes.Select(pe => new EpisodePriceRequest
            //        {
            //            Key = pe.Prices.First().Key,
            //            StartDate = pe.Prices.First().StartDate,
            //            EndDate = pe.Prices.First().EndDate,
            //            Amount = pe.Prices.First().Amount
            //        }).ToList()
            //    }).ToList()
            //}
        };
    }
}