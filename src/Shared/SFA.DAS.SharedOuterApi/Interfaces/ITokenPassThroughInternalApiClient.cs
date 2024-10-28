namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface ITokenPassThroughInternalApiClient<T> : IApiClient<T>
    {
        /// <summary>
        /// Applies a bearer token to the httpContext for the outgoing request
        /// </summary>
        void GenerateServiceToken(string serviceAccount);
    }
}