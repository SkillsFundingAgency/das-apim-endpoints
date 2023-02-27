using System.ServiceModel;

namespace SFA.DAS.SharedOuterApi.ConnectedServices.LearnerWebService
{
    public interface IClientTypeFactory<T>
    {
        T Create(BasicHttpBinding binding);
    }
}