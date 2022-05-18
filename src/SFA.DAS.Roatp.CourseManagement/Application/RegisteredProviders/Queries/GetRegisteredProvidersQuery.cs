using MediatR;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Roatp.CourseManagement.Application.RegisteredProviders.Queries
{
    public class GetRegisteredProvidersQuery : IGetApiRequest, IRequest<GetRegisteredProvidersQueryResult>
    {
        public string GetUrl => $"v1/fat-data-export";
        
    }
}