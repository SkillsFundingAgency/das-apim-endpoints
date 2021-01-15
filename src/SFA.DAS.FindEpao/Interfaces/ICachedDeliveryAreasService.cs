using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.FindEpao.InnerApi.Responses;

namespace SFA.DAS.FindEpao.Interfaces
{
    public interface ICachedDeliveryAreasService
    {
        Task<IReadOnlyList<GetDeliveryAreaListItem>> GetDeliveryAreas();
    }
}