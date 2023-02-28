using System.Security.Cryptography.X509Certificates;
using System;
using Azure.Security.KeyVault.Certificates;
using Azure.Identity;

namespace SFA.DAS.SharedOuterApi.ConnectedServices.LearnerWebService
{
    public class CertificateProvider : ICertificateProvider
    {
        private readonly LrsApiWcfSettings _lrsApiSettings;
        private X509Certificate2 _x509Certificate = null;

        public CertificateProvider(LrsApiWcfSettings lrsApiSettings)
        {
            _lrsApiSettings = lrsApiSettings;
            SetupClientCertificate();
        }

        public X509Certificate2 GetClientCertificate()
        {
            if (string.IsNullOrEmpty(_lrsApiSettings.KeyVaultUrl))
            {
                throw new Exception("KeyVault url is not specified. That is required to run the app");
            }

            if (string.IsNullOrEmpty(_lrsApiSettings.CertName))
            {
                throw new Exception("Cert name added to KeyVault is not specified. That is required to run the app");
            }

            if (_x509Certificate == null)
            {
                SetupClientCertificate();
            }

            return _x509Certificate;
        }

        private void SetupClientCertificate()
        {
            try
            {
                var client = new CertificateClient(new Uri(_lrsApiSettings.KeyVaultUrl), new DefaultAzureCredential(new DefaultAzureCredentialOptions
                {
                    ManagedIdentityClientId = _lrsApiSettings.AzureADManagedIdentityClientId //TODO: do we already have this somewhere else?
                }));

                _x509Certificate = client.DownloadCertificate(_lrsApiSettings.CertName);
            }
            catch (Exception ex)
            {
                //TODO: Logging?
                throw;
            }
        }
    }
}