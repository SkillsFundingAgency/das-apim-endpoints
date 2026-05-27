using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.LearnerData.Requests.EarningsInner;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.LearnerData.Requests;

namespace SFA.DAS.LearnerData.Services;

public interface IUpdateEarningsOnProgrammeRequestBuilder
{
    Task<UpdateOnProgrammeApiPutRequest> Build(Guid learningKey, UpdateLearnerRequest updateLearnerRequest, BaseLearnerApiPutResponse learningApiPutResponse, UpdateLearningApiPutRequest putRequest);
}

public class UpdateEarningsOnProgrammeRequestBuilder(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient) : IUpdateEarningsOnProgrammeRequestBuilder
{
    public async Task<UpdateOnProgrammeApiPutRequest> Build(Guid learningKey, UpdateLearnerRequest updateLearnerRequest, BaseLearnerApiPutResponse learningApiPutResponse,
        UpdateLearningApiPutRequest putRequest)
    {
        var fundingBandMaximum = default(int?);
        var includesFundingBandMaximumUpdate = false;

        if (learningApiPutResponse.Changes.Contains(BaseLearnerApiPutResponse.LearningUpdateChanges.Prices)
            || learningApiPutResponse.Changes.Contains(BaseLearnerApiPutResponse.LearningUpdateChanges.ExpectedEndDate))
        {
            fundingBandMaximum = await GetFundingBandMaximum(updateLearnerRequest);
            includesFundingBandMaximumUpdate = true;
        }

        var payload = new UpdateOnProgrammeRequest
        {
            CompletionDate = putRequest.Data.Learner.CompletionDate,
            WithdrawalDate = putRequest.Data.Delivery.WithdrawalDate,
            PauseDate = putRequest.Data.OnProgramme.PauseDate,
            AchievementDate = putRequest.Data.OnProgramme.AchievementDate,
            ApprenticeshipEpisodeKey = learningApiPutResponse.LearningEpisodeKey,
            FundingBandMaximum = fundingBandMaximum,
            IncludesFundingBandMaximumUpdate = includesFundingBandMaximumUpdate,
            DateOfBirth = putRequest.Data.Learner.DateOfBirth,
            Prices = learningApiPutResponse.Prices.Select(x => new PriceItem
            {
                Key = x.Key,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                TrainingPrice = x.TrainingPrice,
                EndPointAssessmentPrice = x.EndPointAssessmentPrice,
                TotalPrice = x.TotalPrice
            }).ToList(),
            PeriodsInLearning = GetPeriodsInLearning(updateLearnerRequest),
            Care = new SFA.DAS.LearnerData.Requests.EarningsInner.Care
            {
                HasEHCP = putRequest.Data.Learner.Care.HasEHCP,
                IsCareLeaver = putRequest.Data.Learner.Care.IsCareLeaver,
                CareLeaverEmployerConsentGiven = putRequest.Data.Learner.Care.CareLeaverEmployerConsentGiven
            }
        };

        return new UpdateOnProgrammeApiPutRequest(learningKey, payload);
    }

    private async Task<int> GetFundingBandMaximum(UpdateLearnerRequest updateLearnerRequest)
    {
        var onProgramme = updateLearnerRequest.Delivery.OnProgramme.First();
        var standardId = onProgramme.StandardCode.ToString();
        var startDate = onProgramme.StartDate;

        var response = await coursesApiClient.Get<StandardDetailResponse>(new GetStandardDetailsByIdRequest(standardId));

        return response.MaxFundingOn(startDate);
    }

    private List<PeriodInLearningItem> GetPeriodsInLearning(UpdateLearnerRequest updateLearnerRequest)
    {
        var periodsInLearning = new List<PeriodInLearningItem>();

        var agreementId = updateLearnerRequest.Delivery.OnProgramme.First().AgreementId;

        foreach (var onProgramme in updateLearnerRequest.Delivery.OnProgramme.Where(x => x.AgreementId == agreementId))
        {
            //todo:  onProgramme.CompletionDate should be included here in the coalescence. currently left
            //out of here to avoid re-writing the balancing logic in earnings,
            //when we come to do qualification period logic for each PIL we will have to re-write that logic anyway
            //and at that point can include CompletionDate in this calculation
            var endDate = onProgramme.PauseDate ?? onProgramme.WithdrawalDate ?? onProgramme.ExpectedEndDate;

            periodsInLearning.Add(new PeriodInLearningItem
            {
                StartDate = onProgramme.StartDate,
                EndDate = endDate,
                OriginalExpectedEndDate = onProgramme.ExpectedEndDate
            });
        }

        return periodsInLearning.OrderBy(x=>x.StartDate).ToList();
    }
}
