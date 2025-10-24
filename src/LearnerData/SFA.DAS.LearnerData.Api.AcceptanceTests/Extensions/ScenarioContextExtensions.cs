using SFA.DAS.LearnerData.Application.Fm36;
using System.Net.Http.Json;
using TechTalk.SpecFlow;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Extensions;

internal static class ScenarioContextExtensions
{
    internal static GetFm36Result GetFm36ResponseBody(this ScenarioContext context) => context.Get<GetFm36Result>("Fm36ResponseBody");
    internal static List<KeyValuePair<string, IEnumerable<string>>> GetFm36ResponseHeaders(this ScenarioContext context) => context.Get<List<KeyValuePair<string, IEnumerable<string>>>>("Fm36ResponseHeaders");
    internal static async Task SetFm36Response(this ScenarioContext context, HttpResponseMessage response)
    {
        var responseBody = await response.Content.ReadFromJsonAsync<GetFm36Result>();
        var headers = response.Headers.ToList();

        context.Set(responseBody, "Fm36ResponseBody");
        context.Set(headers, "Fm36ResponseHeaders");
    }

}
