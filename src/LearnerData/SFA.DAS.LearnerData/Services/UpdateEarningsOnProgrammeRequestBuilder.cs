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
    Task<UpdateOnProgrammeApiPutRequest> Build(UpdateLearnerRequest updateLearnerRequest, BaseLearnerApiPutResponse learningApiPutResponse, UpdateLearningRequestBody requestBody);
    Task<UpdateOnProgrammeApiPutRequest> Build(Guid learningKey, CreateLearnerRequest createLearnerRequest, BaseLearnerApiPutResponse learningApiPutResponse, UpdateLearningRequestBody requestBody);
}

public class UpdateEarningsOnProgrammeRequestBuilder(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient) : IUpdateEarningsOnProgrammeRequestBuilder
{
    public async Task<UpdateOnProgrammeApiPutRequest> Build(UpdateLearnerRequest updateLearnerRequest, BaseLearnerApiPutResponse learningApiPutResponse,
        UpdateLearningRequestBody requestBody)
    {
        return await BuildInternal(learningApiPutResponse.LearningKey, updateLearnerRequest.Delivery.OnProgramme.Cast<OnProgrammeRequestDetails>().ToList(), learningApiPutResponse, requestBody);
    }

    public async Task<UpdateOnProgrammeApiPutRequest> Build(Guid learningKey, CreateLearnerRequest createLearnerRequest, BaseLearnerApiPutResponse learningApiPutResponse,
        UpdateLearningRequestBody requestBody)
    {
        return await BuildInternal(learningKey, createLearnerRequest.Delivery.OnProgramme.Cast<OnProgrammeRequestDetails>().ToList(), learningApiPutResponse, requestBody);
    }

    private async Task<UpdateOnProgrammeApiPutRequest> BuildInternal(Guid learningKey, List<OnProgrammeRequestDetails> onProgramme, BaseLearnerApiPutResponse learningApiPutResponse, UpdateLearningRequestBody requestBody)
    {
        var fundingBandMaximum = default(int?);
        var includesFundingBandMaximumUpdate = false;

        if (learningApiPutResponse.Changes.Contains(BaseLearnerApiPutResponse.LearningUpdateChanges.Prices)
            || learningApiPutResponse.Changes.Contains(BaseLearnerApiPutResponse.LearningUpdateChanges.ExpectedEndDate))
        {
            fundingBandMaximum = await GetFundingBandMaximum(onProgramme);
            includesFundingBandMaximumUpdate = true;
        }

        var payload = new UpdateOnProgrammeRequest
        {
            CompletionDate = requestBody.Learner.CompletionDate,
            WithdrawalDate = requestBody.Delivery.WithdrawalDate,
            PauseDate = requestBody.OnProgramme.PauseDate,
            AchievementDate = requestBody.OnProgramme.AchievementDate,
            ApprenticeshipEpisodeKey = learningApiPutResponse.LearningEpisodeKey,
            FundingBandMaximum = fundingBandMaximum,
            IncludesFundingBandMaximumUpdate = includesFundingBandMaximumUpdate,
            DateOfBirth = requestBody.Learner.DateOfBirth,
            Prices = learningApiPutResponse.Prices.Select(x => new PriceItem
            {
                Key = x.Key,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                TrainingPrice = x.TrainingPrice,
                EndPointAssessmentPrice = x.EndPointAssessmentPrice,
                TotalPrice = x.TotalPrice
            }).ToList(),
            PeriodsInLearning = GetPeriodsInLearning(onProgramme),
            Care = new SFA.DAS.LearnerData.Requests.EarningsInner.Care
            {
                HasEHCP = requestBody.Learner.Care.HasEHCP,
                IsCareLeaver = requestBody.Learner.Care.IsCareLeaver,
                CareLeaverEmployerConsentGiven = requestBody.Learner.Care.CareLeaverEmployerConsentGiven
            }
        };

        return new UpdateOnProgrammeApiPutRequest(learningKey, payload);
    }

    private async Task<int> GetFundingBandMaximum(List<OnProgrammeRequestDetails> onProgramme)
    {
        var firstOnProgramme = onProgramme.First();
        var standardId = firstOnProgramme.StandardCode.ToString();
        var startDate = firstOnProgramme.StartDate;

        var response = await coursesApiClient.Get<StandardDetailResponse>(new GetStandardDetailsByIdRequest(standardId));

        return response.MaxFundingOn(startDate);
    }

    private List<PeriodInLearningItem> GetPeriodsInLearning(List<OnProgrammeRequestDetails> onProgramme)
    {
        var periodsInLearning = new List<PeriodInLearningItem>();

        var agreementId = onProgramme.First().AgreementId;

        foreach (var onProg in onProgramme.Where(x => x.AgreementId == agreementId))
        {
            var endDate = onProg.PauseDate ?? onProg.WithdrawalDate ?? onProg.CompletionDate;

            periodsInLearning.Add(new PeriodInLearningItem
            {
                StartDate = onProg.StartDate,
                EndDate = endDate,
                OriginalExpectedEndDate = onProg.ExpectedEndDate
            });
        }

        return periodsInLearning.OrderBy(x=>x.StartDate).ToList();
    }
}
