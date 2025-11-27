using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Services.SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;

namespace SFA.DAS.LearnerData.Services
{
    namespace SFA.DAS.LearnerData.Services
    {
        public interface IUpdateLearningApiPutRequestMapper
        {
            UpdateLearningApiPutRequest Map(UpdateLearnerCommand command);
        }
    }

    public class UpdateLearningApiPutRequestMapper(ILearningSupportService learningSupportService, IBreaksInLearningService breaksInLearningService)
        : IUpdateLearningApiPutRequestMapper
    {
        public UpdateLearningApiPutRequest Map(UpdateLearnerCommand command)
        {
            var (firstOnProgramme, latestOnProgramme, allMatchingOnProgrammes) = SelectEpisode(command);

            var breaksInLearning = breaksInLearningService.CalculateOnProgrammeBreaksInLearning(allMatchingOnProgrammes);

            var learningSupport = learningSupportService.GetCombinedLearningSupport(
                allMatchingOnProgrammes,
                command.UpdateLearnerRequest.Delivery.EnglishAndMaths,
                breaksInLearning.Breaks);

            var body = new UpdateLearningRequestBody
            {
                Delivery = new Delivery
                {
                    WithdrawalDate = latestOnProgramme.WithdrawalDate
                },
                Learner = new LearningUpdateDetails
                {
                    FirstName = command.UpdateLearnerRequest.Learner.FirstName,
                    LastName = command.UpdateLearnerRequest.Learner.LastName,
                    EmailAddress = command.UpdateLearnerRequest.Learner.Email,
                    CompletionDate = latestOnProgramme.CompletionDate
                },
                OnProgramme = new OnProgrammeDetails
                {
                    ExpectedEndDate = latestOnProgramme.ExpectedEndDate,
                    Costs = breaksInLearning.Costs.GetCostsOrDefault(firstOnProgramme.StartDate),
                    PauseDate = latestOnProgramme.PauseDate,
                    BreaksInLearning = breaksInLearning.Breaks
                },
                MathsAndEnglishCourses = command.UpdateLearnerRequest.Delivery.EnglishAndMaths.Select(x =>
                    new MathsAndEnglishDetails
                    {
                        Amount = x.Amount,
                        CompletionDate = x.CompletionDate,
                        Course = x.Course,
                        PlannedEndDate = x.EndDate,
                        PriorLearningPercentage = x.PriorLearningPercentage,
                        StartDate = x.StartDate,
                        WithdrawalDate = x.WithdrawalDate
                    }).ToList(),
                LearningSupport = learningSupport
            };

            return new UpdateLearningApiPutRequest(command.LearningKey, body);
        }

        private static (OnProgrammeRequestDetails FirstOnProgramme,
            OnProgrammeRequestDetails LatestOnProgramme,
            List<OnProgrammeRequestDetails> MatchingOnProgrammes) SelectEpisode(UpdateLearnerCommand command)
        {
            var orderedOnProgrammes = command.UpdateLearnerRequest.Delivery.OnProgramme
                .OrderBy(x => x.StartDate)
                .ToList();

            var firstOnProgramme = orderedOnProgrammes.First();

            var allMatchingOnProgrammes = orderedOnProgrammes
                .Where(x => x.StandardCode == firstOnProgramme.StandardCode &&
                            x.AgreementId == firstOnProgramme.AgreementId)
                .ToList();

            var latestOnProgramme = allMatchingOnProgrammes.Last();

            return (firstOnProgramme, latestOnProgramme, allMatchingOnProgrammes);
        }
    }
}
