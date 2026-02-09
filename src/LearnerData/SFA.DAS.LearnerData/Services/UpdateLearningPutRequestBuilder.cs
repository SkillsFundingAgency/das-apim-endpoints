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
            var onProgrammeDetails = BuildOnProgrammeDetails(firstOnProgramme, latestOnProgramme, allMatchingOnProgrammes, costs);
            var englishAndMathsCourses = BuildEnglishAndMathsDetails(command.UpdateLearnerRequest.Delivery.EnglishAndMaths);

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
                onProgrammeDetails.BreaksInLearning,
                englishAndMathsCourses,
                command.EnglishAndMathsLearningSupport());

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
                OnProgramme = onProgrammeDetails,
                MathsAndEnglishCourses = englishAndMathsCourses,
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

        private OnProgrammeDetails BuildOnProgrammeDetails(
            OnProgrammeRequestDetails firstOnProgramme, 
            OnProgrammeRequestDetails latestOnProgramme,
            List<OnProgrammeRequestDetails> allMatchingOnProgrammes,
            List<CostDetails> costs)
        {

            var breaksInLearning = breaksInLearningService.CalculateOnProgrammeBreaksInLearning(allMatchingOnProgrammes);

            return new OnProgrammeDetails
            {
                ExpectedEndDate = latestOnProgramme.ExpectedEndDate,
                Costs = costs.GetCostsOrDefault(firstOnProgramme.StartDate),
                PauseDate = latestOnProgramme.PauseDate,
                BreaksInLearning = breaksInLearning
            };
        }

        private List<MathsAndEnglishDetails> BuildEnglishAndMathsDetails(List<MathsAndEnglish> mathsAndEnglish)
        {
            var groupedCourses = mathsAndEnglish.GroupBy(c => c.LearnAimRef);

            return groupedCourses.Select(g =>
            {
                if(g.Count() == 1)
                {
                    var course = g.First();
                    return new MathsAndEnglishDetails
                    {
                        Amount = course.Amount,
                        CompletionDate = course.CompletionDate,
                        LearnAimRef = course.LearnAimRef,
                        Course = course.Course,
                        PlannedEndDate = course.EndDate,
                        PriorLearningPercentage = course.PriorLearningPercentage,
                        StartDate = course.StartDate,
                        WithdrawalDate = course.WithdrawalDate,
                        PauseDate = course.PauseDate,
                        BreaksInLearning = new List<BreakInLearning>()
                    };
                }
                else
                {
                    var breaksInLearning = breaksInLearningService.CalculateEnglishAndMathsBreaksInLearning(g.ToList());
                    var orderedCourses = g.OrderBy(c => c.StartDate).ToList();
                    var firstCourse = orderedCourses.First();
                    var latestCourse = orderedCourses.Last();

                    return new MathsAndEnglishDetails
                    {
                        Amount = latestCourse.Amount,
                        CompletionDate = latestCourse.CompletionDate,
                        LearnAimRef = latestCourse.LearnAimRef,
                        Course = latestCourse.Course,
                        PlannedEndDate = latestCourse.EndDate,
                        PriorLearningPercentage = latestCourse.PriorLearningPercentage,
                        StartDate = firstCourse.StartDate,
                        WithdrawalDate = latestCourse.WithdrawalDate,
                        PauseDate = latestCourse.PauseDate,
                        BreaksInLearning = breaksInLearning
                    };
                }
            }).ToList();
        }
    }
}
