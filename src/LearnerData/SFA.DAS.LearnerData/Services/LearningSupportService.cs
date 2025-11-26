using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;

namespace SFA.DAS.LearnerData.Services
{
    public interface ILearningSupportService
    {
        List<LearningSupportUpdatedDetails> GetCombinedLearningSupport(UpdateLearnerRequest request);
    }

    public class LearningSupportService : ILearningSupportService
    {
        public List<LearningSupportUpdatedDetails> GetCombinedLearningSupport(UpdateLearnerRequest request)
        {
            var combined = new List<LearningSupportUpdatedDetails>();

            combined.AddRange(GetOnProgrammeLearningSupport(request));
            combined.AddRange(GetEnglishAndMathsLearningSupport(request));

            return combined;
        }

        private static IEnumerable<LearningSupportUpdatedDetails> GetOnProgrammeLearningSupport(UpdateLearnerRequest request)
        {
            var results = new List<LearningSupportUpdatedDetails>();

            foreach (var onProg in request.Delivery.OnProgramme)
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

        private static IEnumerable<LearningSupportUpdatedDetails> GetEnglishAndMathsLearningSupport(UpdateLearnerRequest request)
        {
            var results = new List<LearningSupportUpdatedDetails>();

            foreach (var em in request.Delivery.EnglishAndMaths)
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

