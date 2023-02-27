using LearnerServiceClient;
using System.ServiceModel;

namespace SFA.DAS.SharedOuterApi.ConnectedServices.LearnerWebService
{
    public class LearnerPortTypeClientFactory : IClientTypeFactory<LearnerPortTypeClient>
    {
        private readonly LrsApiWcfSettings _lrsApiSettings;
        private readonly ICertificateProvider _certificateProvider;

        public LearnerPortTypeClientFactory(LrsApiWcfSettings lrsApiSettings, ICertificateProvider certificateProvider)
        {
            _lrsApiSettings = lrsApiSettings;
            _certificateProvider = certificateProvider;
        }

        public LearnerPortTypeClient Create(BasicHttpBinding binding)
        {
            var client = new LearnerPortTypeClient(binding, new EndpointAddress(_lrsApiSettings.LearnerServiceBaseUrl));
            client.ClientCredentials.ClientCertificate.Certificate = _certificateProvider.GetClientCertificate();
            return client;
        }
    }
}
