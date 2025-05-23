﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TechTalk.SpecFlow;

namespace SFA.DAS.Earnings.Api.AcceptanceTests.Bindings
{
    [Binding]
    public class OuterApi
    {
        public static HttpClient Client { get; set; }
        public static LocalWebApplicationFactory<Startup> Factory { get; set; }
        public static Dictionary<string, string> Config { get; set; }


        private readonly TestContext _context;

        public OuterApi(TestContext context)
        {
            _context = context;
        }
        
        [BeforeScenario()]
        public void Initialise()
        {
            NUnit.Framework.TestContext.WriteLine("Initialising...");

            if (Client == null)
            {
                Config = new Dictionary<string, string>
                {
                    {"Environment", "LOCAL_ACCEPTANCE_TESTS"},
                    {"EarningsApiConfiguration:url", _context?.EarningsApi?.BaseAddress + "/"},
                    {"ApprenticeshipsApiConfiguration:url", _context?.ApprenticeshipsApi?.BaseAddress + "/"},
                    {"ApprenticeshipsApiConfiguration:BearerTokenSigningKey", "local_test_outer_api_client_bearer_token_signing_key"},
                    {"CollectionCalendarApiConfiguration:url", _context?.CollectionCalendarApi?.BaseAddress + "/"},
                    {"AzureAD:tenant", ""},
                    {"AzureAD:identifier", ""}
                };


                Factory = new LocalWebApplicationFactory<Startup>(Config);
                Client = Factory.CreateClient();
                Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GenerateBearerToken("Earnings")}" );
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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["ApprenticeshipsApiConfiguration:BearerTokenSigningKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
