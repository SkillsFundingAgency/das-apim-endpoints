using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;

namespace SFA.DAS.LearnerData.Services
{
    public interface ILearningSupportService
    {
        List<LearningSupportUpdatedDetails> GetCombinedLearningSupport(List<OnProgrammeRequestDetails> onProgrammeRequestDetailsList, List<MathsAndEnglish> englishAndMaths, List<BreakInLearning> breakInLearnings);
    }

    public class LearningSupportService : ILearningSupportService
    {
        public List<LearningSupportUpdatedDetails> GetCombinedLearningSupport(List<OnProgrammeRequestDetails> onProgrammeRequestDetailsList, List<MathsAndEnglish> englishAndMaths, List<BreakInLearning> breakInLearnings)
        {
            var combined = new List<LearningSupportUpdatedDetails>();

            combined.AddRange(GetOnProgrammeLearningSupport(onProgrammeRequestDetailsList));
            combined.AddRange(GetEnglishAndMathsLearningSupport(englishAndMaths));

            return combined;
        }

        private static IEnumerable<LearningSupportUpdatedDetails> GetOnProgrammeLearningSupport(List<OnProgrammeRequestDetails> onProgrammeRequestDetailsList)
        {
            var results = new List<LearningSupportUpdatedDetails>();

            foreach (var onProg in onProgrammeRequestDetailsList)
            {
                foreach (var ls in onProg.LearningSupport)
                {
                    var potentialEndDates = new List<DateTime> { ls.EndDate };

                    if (onProg.CompletionDate.HasValue)
                        potentialEndDates.Add(onProg.CompletionDate.Value);

                    if (onProg.WithdrawalDate.HasValue)
                        potentialEndDates.Add(onProg.WithdrawalDate.Value);

                    if (onProg.PauseDate.HasValue)
                        potentialEndDates.Add(onProg.PauseDate.Value);

                    var endDate = potentialEndDates.Min();

                    results.Add(new LearningSupportUpdatedDetails
                    {
                        StartDate = ls.StartDate,
                        EndDate = endDate
                    });
                }
            }

            return results;
        }

        private static IEnumerable<LearningSupportUpdatedDetails> GetEnglishAndMathsLearningSupport(List<MathsAndEnglish> englishAndMaths)
        {
            var results = new List<LearningSupportUpdatedDetails>();

            foreach (var em in englishAndMaths)
            {
                foreach (var ls in em.LearningSupport)
                {
                    var potentialEndDates = new List<DateTime> { ls.EndDate };

                    if (em.CompletionDate.HasValue)
                        potentialEndDates.Add(em.CompletionDate.Value);

                    if (em.WithdrawalDate.HasValue)
                        potentialEndDates.Add(em.WithdrawalDate.Value);

                    var endDate = potentialEndDates.Min();

                    results.Add(new LearningSupportUpdatedDetails
                    {
                        StartDate = ls.StartDate,
                        EndDate = endDate
                    });
                }
            }

            return results;
        }

    }
}

