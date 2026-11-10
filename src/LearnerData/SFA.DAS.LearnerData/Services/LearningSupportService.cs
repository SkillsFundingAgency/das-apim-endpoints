using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Shared;

namespace SFA.DAS.LearnerData.Services;

public interface ILearningSupportService
{
    (List<LearningSupport> OnProgramme, Dictionary<string, List<LearningSupport>> EnglishAndMaths) GetLearningSupport(
        List<OnProgrammeRequestDetails> onProgrammes,
        DateTime onProgrammeEndDate,
        List<BreakInLearning> onProgrammeBreaksInLearning,
        List<MathsAndEnglishDetails> englishAndMathsCourses,
        IEnumerable<KeyValuePair<string, List<LearningSupport>>> englishAndMathsRequestedLearningSupportByLearnAimRef);
}

public class LearningSupportService : ILearningSupportService
{
    public (List<LearningSupport> OnProgramme, Dictionary<string, List<LearningSupport>> EnglishAndMaths) GetLearningSupport(
        List<OnProgrammeRequestDetails> onProgrammes,
        DateTime onProgrammeEndDate,
        List<BreakInLearning> onProgrammeBreaksInLearning,
        List<MathsAndEnglishDetails> englishAndMathsCourses,
        IEnumerable<KeyValuePair<string, List<LearningSupport>>> englishAndMathsRequestedLearningSupportByLearnAimRef)
    {
        var onProgrammeLearningSupport = ProcessLearningSupport(onProgrammes.SelectMany(op => op.LearningSupport), onProgrammeBreaksInLearning, onProgrammeEndDate);

        var englishAndMathsLearningSupport = new Dictionary<string, List<LearningSupport>>();

        foreach(var course in englishAndMathsCourses)
        {
            var requestedLearningSupport = englishAndMathsRequestedLearningSupportByLearnAimRef
                .Where(kvp => kvp.Key == course.LearnAimRef)
                .SelectMany(kvp => kvp.Value);

            if (requestedLearningSupport.Any())
            {
                var endDate = new[]
                {
                    course.PlannedEndDate,
                    course.CompletionDate,
                    course.WithdrawalDate,
                    course.PauseDate
                }.Min();
                englishAndMathsLearningSupport[course.LearnAimRef] = ProcessLearningSupport(requestedLearningSupport!, course.BreaksInLearning, endDate!.Value);
            }

        }

        return (onProgrammeLearningSupport, englishAndMathsLearningSupport);
    }

    private static List<LearningSupport> ProcessLearningSupport(
        IEnumerable<LearningSupport> requestedLearningSupport,
        List<BreakInLearning> breaksInLearning,
        DateTime effectiveEndDate)
    {
        return requestedLearningSupport
            //split & truncate ls by breaks
            .SelectMany(ls => SplitByBreaks(
                new LearningSupport { StartDate = ls.StartDate, EndDate = ls.EndDate },
                breaksInLearning))
            //truncate ls by learning end date
            .Select(seg => Truncate(seg, effectiveEndDate))
            //drop zero-length segments
            .Where(seg => seg.EndDate >= seg.StartDate)
            .ToList();
    }

    private static List<LearningSupport> SplitByBreaks(
        LearningSupport segment,
        IEnumerable<BreakInLearning> breaks)
    {
        var segments = new List<LearningSupport> { segment };

        foreach (var bil in breaks)
        {
            segments = segments.SelectMany(seg => SplitSegment(seg, bil)).ToList();
        }

        return segments;
    }

    private static IEnumerable<LearningSupport> SplitSegment(LearningSupport seg, BreakInLearning bil)
    {
        //segment not truncated by break
        if (seg.EndDate < bil.StartDate || seg.StartDate > bil.EndDate)
        {
            yield return seg;
        }
        //split ls if a break falls fully within
        else if (seg.StartDate < bil.StartDate && seg.EndDate > bil.EndDate)
        {
            yield return new LearningSupport
            {
                StartDate = seg.StartDate,
                EndDate = bil.StartDate.AddDays(-1)
            };
            yield return new LearningSupport
            {
                StartDate = bil.EndDate.AddDays(1),
                EndDate = seg.EndDate
            };
        }
        //truncate end date with start of breaks
        else if (seg.StartDate < bil.StartDate && seg.EndDate <= bil.EndDate)
        {
            yield return new LearningSupport
            {
                StartDate = seg.StartDate,
                EndDate = bil.StartDate.AddDays(-1)
            };
        }
        //truncate start date with end of breaks
        else if (seg.StartDate >= bil.StartDate && seg.StartDate <= bil.EndDate && seg.EndDate > bil.EndDate)
        {
            yield return new LearningSupport
            {
                StartDate = bil.EndDate.AddDays(1),
                EndDate = seg.EndDate
            };
        }
        // do not yield segments fully inside break
    }

    private static LearningSupport Truncate(LearningSupport seg, DateTime effectiveEndDate)
    {
        return new LearningSupport
        {
            StartDate = seg.StartDate,
            EndDate = effectiveEndDate < seg.EndDate ? effectiveEndDate : seg.EndDate
        };
    }

}
