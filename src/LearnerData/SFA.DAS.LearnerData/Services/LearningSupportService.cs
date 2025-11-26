using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;

namespace SFA.DAS.LearnerData.Services
{
    public interface ILearningSupportService
    {
        List<LearningSupportUpdatedDetails> GetCombinedLearningSupport(List<OnProgrammeRequestDetails> onProgrammeRequestDetailsList, List<MathsAndEnglish> englishAndMaths, List<BreakInLearning> breaksInLearning);
    }

    public class LearningSupportService : ILearningSupportService
    {
        public List<LearningSupportUpdatedDetails> GetCombinedLearningSupport(List<OnProgrammeRequestDetails> onProgrammeRequestDetailsList, List<MathsAndEnglish> englishAndMaths, List<BreakInLearning> breaksInLearning)
        {
            var combined = new List<LearningSupportUpdatedDetails>();

            combined.AddRange(GetOnProgrammeLearningSupport(onProgrammeRequestDetailsList, breaksInLearning));
            combined.AddRange(GetEnglishAndMathsLearningSupport(englishAndMaths));

            return combined;
        }

        private static IEnumerable<LearningSupportUpdatedDetails> GetOnProgrammeLearningSupport(
    List<OnProgrammeRequestDetails> onProgrammeRequestDetailsList,
    List<BreakInLearning> breaksInLearning)
        {
            var results = new List<LearningSupportUpdatedDetails>();

            // Build a basic list of LSF, each truncated by potential end dates
            foreach (var onProg in onProgrammeRequestDetailsList)
            {
                foreach (var ls in onProg.LearningSupport)
                {
                    var potentialEndDates = new List<DateTime> { ls.EndDate };

                    if (onProg.CompletionDate.HasValue) potentialEndDates.Add(onProg.CompletionDate.Value);
                    if (onProg.WithdrawalDate.HasValue) potentialEndDates.Add(onProg.WithdrawalDate.Value);
                    if (onProg.PauseDate.HasValue) potentialEndDates.Add(onProg.PauseDate.Value);

                    var endDate = potentialEndDates.Min();

                    results.Add(new LearningSupportUpdatedDetails
                    {
                        StartDate = ls.StartDate,
                        EndDate = endDate
                    });
                }
            }

            var adjusted = new List<LearningSupportUpdatedDetails>();

            foreach (var ls in results)
            {
                var segments = new List<LearningSupportUpdatedDetails> { ls };

                foreach (var bil in breaksInLearning)
                {
                    var newSegments = new List<LearningSupportUpdatedDetails>();

                    foreach (var seg in segments)
                    {
                        // fully before break
                        if (seg.EndDate < bil.StartDate)
                        {
                            newSegments.Add(seg);
                        }
                        // fully after break
                        else if (seg.StartDate > bil.EndDate)
                        {
                            newSegments.Add(seg);
                        }
                        // spans break → split
                        else if (seg.StartDate < bil.StartDate && seg.EndDate > bil.EndDate)
                        {
                            newSegments.Add(new LearningSupportUpdatedDetails
                            {
                                StartDate = seg.StartDate,
                                EndDate = bil.StartDate.AddDays(-1)
                            });
                            newSegments.Add(new LearningSupportUpdatedDetails
                            {
                                StartDate = bil.EndDate.AddDays(1),
                                EndDate = seg.EndDate
                            });
                        }
                        // overlaps break start only → truncate end
                        else if (seg.StartDate < bil.StartDate && seg.EndDate <= bil.EndDate)
                        {
                            newSegments.Add(new LearningSupportUpdatedDetails
                            {
                                StartDate = seg.StartDate,
                                EndDate = bil.StartDate.AddDays(-1)
                            });
                        }
                        // overlaps break end only → truncate start
                        else if (seg.StartDate >= bil.StartDate && seg.StartDate <= bil.EndDate && seg.EndDate > bil.EndDate)
                        {
                            newSegments.Add(new LearningSupportUpdatedDetails
                            {
                                StartDate = bil.EndDate.AddDays(1),
                                EndDate = seg.EndDate
                            });
                        }
                        // fully inside break → drop
                    }

                    segments = newSegments;
                }

                adjusted.AddRange(segments);
            }

            // remove zero-length segments
            adjusted = adjusted.Where(r => r.EndDate >= r.StartDate).ToList();

            return adjusted;
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

