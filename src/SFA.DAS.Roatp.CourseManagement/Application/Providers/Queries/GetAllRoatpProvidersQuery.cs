using MediatR;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Roatp.CourseManagement.Application.Providers.Queries
{
    public class GetAllRoatpProvidersQuery : IGetApiRequest, IRequest<GetAllRoatpProvidersQueryResult>
    {
        public string GetUrl => $"v1/fat-data-export";
        
    }
}