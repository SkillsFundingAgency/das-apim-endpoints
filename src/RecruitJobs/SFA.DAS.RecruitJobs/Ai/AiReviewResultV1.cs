using System.Collections.Generic;
using System.Linq;
using System.Net;
using SFA.DAS.RecruitJobs.Ai.Clients;
using SFA.DAS.SharedOuterApi.Extensions;

namespace SFA.DAS.RecruitJobs.Ai;

public class AiReviewResultV1: AiReviewResult
{
    public AzureAiResponse<Dictionary<string, string>> SpellcheckResult { get; set; }
    public AzureAiResponse<Dictionary<string, string>> DiscriminationResult { get; set; }
    public AzureAiResponse<Dictionary<string, string>> ContentEvaluationResult { get; set; }

    private static double GetStatusScore(HttpStatusCode? status)
    {
        return status is null 
            ? 1 
            : status.Value.IsSuccessStatusCode() ? 0 : 1;
    }

    private double GetSpellcheckScore()
    {
        var total = GetStatusScore(SpellcheckResult?.StatusCode);
        var anyErrors = SpellcheckResult?.Result?.Any(x => !string.IsNullOrWhiteSpace(x.Value)) ?? false;
        return total + (anyErrors ? 0.5 : 0);
    }

    public override double GetScore()
    {
        var spellCheckScore = GetSpellcheckScore();
        var discriminationScore = GetStatusScore(DiscriminationResult?.StatusCode) + (DiscriminationResult?.Result?.Count(x => !string.IsNullOrWhiteSpace(x.Value)) ?? 1);
        var contentScore = GetStatusScore(ContentEvaluationResult?.StatusCode) + (ContentEvaluationResult?.Result?.Count(x => !string.IsNullOrWhiteSpace(x.Value)) ?? 1);
        return spellCheckScore + discriminationScore + contentScore;
    }
}