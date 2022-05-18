using System.Collections.Generic;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Models.ProviderRegistration;

namespace SFA.DAS.Roatp.CourseManagement.Application.RegisteredProviders.Queries
{
    public class GetRegisteredProvidersQueryResult
    {
        public List<RegisteredProviderModel> Providers { get; set; }
    }
}