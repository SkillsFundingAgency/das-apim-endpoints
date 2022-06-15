using System.Collections.Generic;
using MediatR;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models.RegisteredProvider;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.RoatpCourseManagement.Application.RegisteredProviders.Queries
{
    public class GetRegisteredProvidersQuery : IGetApiRequest, IRequest<ApiResponse<List<RegisteredProviderModel>>>
    {
        public string GetUrl => $"api/v1/fat-data-export";
    }
}