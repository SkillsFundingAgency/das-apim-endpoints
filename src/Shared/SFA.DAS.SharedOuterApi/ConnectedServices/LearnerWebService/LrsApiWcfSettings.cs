namespace SFA.DAS.SharedOuterApi.ConnectedServices.LearnerWebService
{
    public class LrsApiWcfSettings
    {
        public string LearnerServiceBaseUrl { get; set; }

        public string KeyVaultUrl { get; set; }

        public string CertName { get; set; }

        public string AzureADManagedIdentityClientId { get; set; }
    }
}