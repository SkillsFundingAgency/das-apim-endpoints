using System.Globalization;
using System.Text.RegularExpressions;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace SFA.DAS.FindAnApprenticeship.MockServer.MockServerBuilder;

public static class FindAnApprenticeshipMock
{
    public static WireMockServer WithFindAnApprenticeshipFiles(this WireMockServer server)
    {
        var regexMaxTimeOut = TimeSpan.FromSeconds(3);
        server.Given(Request.Create()
                .WithPath(s => Regex.IsMatch(s, "/api/vacancies/VAC\\d{10}$", RegexOptions.None, regexMaxTimeOut) 
                               || Regex.IsMatch(s, "/api/vacancies/\\d{10}$", RegexOptions.None, regexMaxTimeOut))
                .UsingGet())
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(message =>
                    {
                        var content = File.ReadAllText("vacancy-detail.json");
                        var vacancyRef = message.Url.Substring(message.Url.LastIndexOf('/') + 1);
                        var id = vacancyRef.Replace("VAC", "");
                        content = content.Replace("{{VACANCY_REF}}", vacancyRef);
                        content = content.Replace("{{ID}}", id);
                        content = content.Replace("{{CLOSING_DATE}}", DateTime.UtcNow.AddMonths(3).ToString("yyyy-MM-ddTHH:mm:ssZ"));
                        content = content.Replace("{{START_DATE}}", DateTime.UtcNow.AddMonths(4).ToString("yyyy-MM-ddTHH:mm:ssZ"));
                        return content;
                    }));

        return server;
    }
}