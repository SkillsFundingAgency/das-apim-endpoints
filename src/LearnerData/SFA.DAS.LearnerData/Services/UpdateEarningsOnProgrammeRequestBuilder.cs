using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.Services
{
    public interface IUpdateEarningsOnProgrammeRequestBuilder
    {
        Task<UpdateOnProgrammeApiPutRequest> Build(UpdateLearnerCommand command, UpdateLearnerApiPutResponse learningApiPutResponse, UpdateLearningApiPutRequest putRequest);
    }

    public class UpdateEarningsOnProgrammeRequestBuilder(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient) : IUpdateEarningsOnProgrammeRequestBuilder
    {
        public async Task<UpdateOnProgrammeApiPutRequest> Build(UpdateLearnerCommand command, UpdateLearnerApiPutResponse learningApiPutResponse,
            UpdateLearningApiPutRequest putRequest)
        {
            var fundingBandMaximum = default(int?);
            var includesFundingBandMaximumUpdate = false;

            if (learningApiPutResponse.Changes.Contains(UpdateLearnerApiPutResponse.LearningUpdateChanges.Prices)
                || learningApiPutResponse.Changes.Contains(UpdateLearnerApiPutResponse.LearningUpdateChanges.ExpectedEndDate))
            {
                fundingBandMaximum = await GetFundingBandMaximum(command);
                includesFundingBandMaximumUpdate = true;
            }

            var payload = new UpdateOnProgrammeRequest
            {
                CompletionDate = putRequest.Data.Learner.CompletionDate,
                WithdrawalDate = putRequest.Data.Delivery.WithdrawalDate,
                PauseDate = putRequest.Data.OnProgramme.PauseDate,
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
                BreaksInLearning = putRequest.Data.OnProgramme.BreaksInLearning.Select(x => new BreakInLearningItem
                {
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    PriorPeriodExpectedEndDate = x.PriorPeriodExpectedEndDate
                }).ToList()
            };

            return new UpdateOnProgrammeApiPutRequest(command.LearningKey, payload);
        }

        private async Task<int> GetFundingBandMaximum(UpdateLearnerCommand command)
        {
            var onProgramme = command.UpdateLearnerRequest.Delivery.OnProgramme.First();
            var standardId = onProgramme.StandardCode.ToString();
            var startDate = onProgramme.StartDate;

            var response = await coursesApiClient.Get<StandardDetailResponse>(new GetStandardDetailsByIdRequest(standardId));

            return response.MaxFundingOn(startDate);
        }
    }
}
