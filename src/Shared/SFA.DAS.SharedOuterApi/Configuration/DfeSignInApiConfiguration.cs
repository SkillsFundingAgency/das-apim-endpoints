using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Configuration
{
    public class DfeSignInApiConfiguration: IApiConfiguration
    {
        public string Url { get; set; }
        public string Audience { get; set; } //aud
        public string ClientId { get; set; } // iss
        public string ApiSecret { get; set; } 
        public int TokenLifetimeMinutes { get; set; } = 5;
        public string QfauUkprn { get; set; } 
    }
}
