using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface IAzureClientCredentialHelper
    {
        Task<string> GetAccessTokenAsync(string identifier);
    }
}