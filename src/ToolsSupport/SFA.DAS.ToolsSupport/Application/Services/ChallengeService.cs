using System.Security.Cryptography;
using SFA.DAS.ToolsSupport.Application.Queries.GetChallenge;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using SFA.DAS.ToolsSupport.Models;


namespace SFA.DAS.ToolsSupport.Application.Services;
public class ChallengeService : IChallengeService
{
    public GetChallengeQueryResult GetChallengeQueryResultFromAccount(Account account, IEnumerable<PayeScheme> payeSchemes)
    {
        if (account != null)
        {
            return new GetChallengeQueryResult
            {
                StatusCode = SearchResponseCodes.Success,
                Account = account,
                Characters = GetPayeSchemesCharacters(payeSchemes)
            };
        }

        return new GetChallengeQueryResult { StatusCode = SearchResponseCodes.NoSearchResultsFound };
    }

    public List<int> GetPayeSchemesCharacters(IEnumerable<PayeScheme> payeSchemes)
    {
        var payeSchemeModels = payeSchemes as PayeScheme[] ?? payeSchemes.ToArray();

        if (!payeSchemeModels.Any())
        {
            return [];
        }

        var schemes = payeSchemeModels
            .Select(p => p.ObscuredPayeRef.Substring(1, p.ObscuredPayeRef.Length - 2).Replace("/", string.Empty))
            .ToList();

        var range = GetMinimumNumberOfCharacters(schemes);

        var response = GetRandomPositions(range + 1);

        response.Sort();

        return response;
    }

    private static List<int> GetRandomPositions(int range)
    {
        var random1 = RandomNumberGenerator.GetInt32(1, range);

        int random2;

        do
        {
            random2 = RandomNumberGenerator.GetInt32(1, range);
        } while (random1 == random2);

        return
        [
            random1,
            random2
        ];
    }

    private static int GetMinimumNumberOfCharacters(IEnumerable<string> schemes)
    {
        return schemes.Select(scheme => scheme.Length).Min();
    }
}

public interface IChallengeService
{
    GetChallengeQueryResult GetChallengeQueryResultFromAccount(Account account, IEnumerable<PayeScheme> payeSchemes);
    List<int> GetPayeSchemesCharacters(IEnumerable<PayeScheme> payeSchemes);
}