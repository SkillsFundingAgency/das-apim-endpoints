using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Reqnroll;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Bindings;

[Binding]
public class OuterApi
{
    public static HttpClient Client { get; set; }
    public static LocalWebApplicationFactory<Program> Factory { get; set; }
    public static Dictionary<string, string> Config { get; set; }


    private readonly TestContext _context;

    public OuterApi(TestContext context)
    {
        _context = context;
    }

    [BeforeScenario(Order = 2)]
    public void Initialise()
    {
        NUnit.Framework.TestContext.WriteLine("Initialising...");

        if (Client == null)
        {
            Config = new Dictionary<string, string>
            {
                {"Environment", "LOCAL_ACCEPTANCE_TESTS"},
                {"EarningsApiConfiguration:url", _context?.EarningsApi?.BaseAddress + "/"},
                {"LearningApiConfiguration:url", _context?.ApprenticeshipsApi?.BaseAddress + "/"},
                {"LearningApiConfiguration:BearerTokenSigningKey", "local_test_outer_api_client_bearer_token_signing_key"},
                {"CollectionCalendarApiConfiguration:url", _context?.CollectionCalendarApi?.BaseAddress + "/"},
                {"AzureAD:tenant", ""},
                {"AzureAD:identifier", ""}
            };


            Factory = new LocalWebApplicationFactory<Program>(Config);
            Client = Factory.CreateClient();
            Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GenerateBearerToken("Earnings")}");
        }

        _context.OuterApiClient = Client;
    }

    private static string GenerateBearerToken(string serviceAccount)
    {
        var claims = new[]
        {
            new Claim("serviceAccount", serviceAccount),
            new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddMinutes(60).ToUnixTimeSeconds().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["LearningApiConfiguration:BearerTokenSigningKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
