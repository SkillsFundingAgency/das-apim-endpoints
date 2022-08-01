using MediatR;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Roatp.CourseManagement.Application.Regions.Queries
{
    public class GetAllRegionsQuery : IGetApiRequest, IRequest<GetAllRegionsQueryResult>
    {
        public string GetUrl => $"lookup/regions";
    }
}
