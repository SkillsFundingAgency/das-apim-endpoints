namespace SFA.DAS.SharedOuterApi.ConnectedServices.LearnerWebService
{
    public interface ILearnerServiceClientProvider<T>
    {
        T GetServiceAsync();
    }
}