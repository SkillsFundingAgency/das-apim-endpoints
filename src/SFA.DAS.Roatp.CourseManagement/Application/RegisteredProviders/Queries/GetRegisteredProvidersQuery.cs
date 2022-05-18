using System.Collections.Generic;
using MediatR;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Models.RegisteredProvider;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Roatp.CourseManagement.Application.RegisteredProviders.Queries
{
    public class GetRegisteredProvidersQuery : IGetApiRequest, IRequest<ApiResponse<List<RegisteredProviderModel>>>
    {
        public string GetUrl => $"v1/fat-data-export";
        
    }
}