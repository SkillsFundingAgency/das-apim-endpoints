using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Interfaces
{
    public interface IAzureClientCredentialHelper
    {
        Task<string> GetAccessTokenAsync();
    }
}