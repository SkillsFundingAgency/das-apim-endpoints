namespace SFA.DAS.SharedOuterApi.Configuration
{

    public class AccessTokenProviderApiConfiguration
    {
        public string Url { get; set; }
        public string Scope { get; set; }
        public string ClientId { get; set; }
        public string Tenant { get; set; }
        public string ClientSecret { get; set; }
        public string GrantType { get; set; }
        public bool ShouldSkipForLocal { get; set; }
    }
}
