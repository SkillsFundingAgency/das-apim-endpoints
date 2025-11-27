using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;

namespace SFA.DAS.LearnerData.Services
{
    public interface ILearningSupportService
    {
        List<LearningSupportUpdatedDetails> GetCombinedLearningSupport(List<OnProgrammeRequestDetails> onProgrammeRequestDetailsList, DateTime onProgrammeEndDate, List<MathsAndEnglish> englishAndMaths, List<BreakInLearning> breaksInLearning);
    }

    public class LearningSupportService : ILearningSupportService
    {
        public List<LearningSupportUpdatedDetails> GetCombinedLearningSupport(List<OnProgrammeRequestDetails> onProgrammeRequestDetailsList, DateTime onProgrammeEndDate, List<MathsAndEnglish> englishAndMaths, List<BreakInLearning> breaksInLearning)
        {
            var combined = new List<LearningSupportUpdatedDetails>();

            combined.AddRange(GetOnProgrammeLearningSupport(onProgrammeRequestDetailsList, breaksInLearning, onProgrammeEndDate));
            combined.AddRange(GetEnglishAndMathsLearningSupport(englishAndMaths));

            return combined;
        }

        private static IEnumerable<LearningSupportUpdatedDetails> GetOnProgrammeLearningSupport(
    List<OnProgrammeRequestDetails> onProgrammeRequestDetailsList,
    List<BreakInLearning> breaksInLearning,
    DateTime effectiveEndDate)
        {
            var adjusted = new List<LearningSupportUpdatedDetails>();

            // get all learning support items across programmes
            var allLearningSupport = onProgrammeRequestDetailsList.SelectMany(x => x.LearningSupport);

            foreach (var ls in allLearningSupport)
            {
                // start with the raw LSF span
                var initialSeg = new LearningSupportUpdatedDetails
                {
                    StartDate = ls.StartDate,
                    EndDate = ls.EndDate
                };

                var segments = new List<LearningSupportUpdatedDetails> { initialSeg };

                // split by breaks
                foreach (var bil in breaksInLearning)
                {
                    var newSegments = new List<LearningSupportUpdatedDetails>();

                    foreach (var seg in segments)
                    {
                        if (seg.EndDate < bil.StartDate)
                        {
                            newSegments.Add(seg);
                        }
                        else if (seg.StartDate > bil.EndDate)
                        {
                            newSegments.Add(seg);
                        }
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
                        else if (seg.StartDate < bil.StartDate && seg.EndDate <= bil.EndDate)
                        {
                            newSegments.Add(new LearningSupportUpdatedDetails
                            {
                                StartDate = seg.StartDate,
                                EndDate = bil.StartDate.AddDays(-1)
                            });
                        }
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

                // truncate each segment to effectiveEndDate
                foreach (var seg in segments)
                {
                    adjusted.Add(new LearningSupportUpdatedDetails
                    {
                        StartDate = seg.StartDate,
                        EndDate = effectiveEndDate < seg.EndDate ? effectiveEndDate : seg.EndDate
                    });
                }
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

