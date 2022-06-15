using System.Threading.Tasks;
using SFA.DAS.ApimDeveloper.InnerApi.Responses;

namespace SFA.DAS.ApimDeveloper.Interfaces
{
    public interface IApimApiService
    {
        Task<GetAvailableApiProductsResponse> GetAvailableProducts(string accountType);
    }
}