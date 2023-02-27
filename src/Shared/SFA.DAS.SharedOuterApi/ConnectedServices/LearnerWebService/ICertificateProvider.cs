using System.Security.Cryptography.X509Certificates;

namespace SFA.DAS.SharedOuterApi.ConnectedServices.LearnerWebService
{
    public interface ICertificateProvider
    {
        X509Certificate2 GetClientCertificate();
    }
}