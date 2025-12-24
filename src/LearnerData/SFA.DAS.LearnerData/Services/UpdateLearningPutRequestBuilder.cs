using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Services.SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;

namespace SFA.DAS.LearnerData.Services
{
    namespace SFA.DAS.LearnerData.Services
    {
        public interface IUpdateLearningPutRequestBuilder
        {
            UpdateLearningApiPutRequest Build(UpdateLearnerCommand command);
        }
    }

    public class UpdateLearningPutRequestBuilder(
        ILearningSupportService learningSupportService,
        IBreaksInLearningService breaksInLearningService,
        ICostsService costsService)
        : IUpdateLearningPutRequestBuilder
    {
        public UpdateLearningApiPutRequest Build(UpdateLearnerCommand command)
        {
            var (firstOnProgramme, latestOnProgramme, allMatchingOnProgrammes) = SelectEpisode(command);

            var costs = costsService.GetCosts(allMatchingOnProgrammes);
            var breaksInLearning = breaksInLearningService.CalculateOnProgrammeBreaksInLearning(allMatchingOnProgrammes);

            //Determine the effective end date of the latest OnProgramme
            var onProgrammeEndDate = new[]
            {
                latestOnProgramme.ExpectedEndDate,
                latestOnProgramme.CompletionDate ?? DateTime.MaxValue,
                latestOnProgramme.WithdrawalDate ?? DateTime.MaxValue,
                latestOnProgramme.PauseDate ?? DateTime.MaxValue
            }.Min();

            var learningSupport = learningSupportService.GetCombinedLearningSupport(
                allMatchingOnProgrammes,
                onProgrammeEndDate,
                command.UpdateLearnerRequest.Delivery.EnglishAndMaths,
                breaksInLearning);

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
                    CompletionDate = latestOnProgramme.CompletionDate,
                    DateOfBirth = command.UpdateLearnerRequest.Learner.Dob,
                    Care = new CareDetails
                    {
                        HasEHCP = command.UpdateLearnerRequest.Learner.HasEhcp,
                        IsCareLeaver = latestOnProgramme.Care.Careleaver,
                        CareLeaverEmployerConsentGiven = latestOnProgramme.Care.EmployerConsent
                    }
                },
                OnProgramme = new OnProgrammeDetails
                {
                    ExpectedEndDate = latestOnProgramme.ExpectedEndDate,
                    Costs = costs.GetCostsOrDefault(firstOnProgramme.StartDate),
                    PauseDate = latestOnProgramme.PauseDate,
                    BreaksInLearning = breaksInLearning
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
                        WithdrawalDate = x.WithdrawalDate,
                        PauseDate = x.PauseDate
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
