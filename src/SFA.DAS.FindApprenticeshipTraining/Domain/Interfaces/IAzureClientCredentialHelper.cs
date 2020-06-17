using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Domain.Interfaces
{
    public interface IAzureClientCredentialHelper
    {
        Task<string> GetAccessTokenAsync();
    }
}