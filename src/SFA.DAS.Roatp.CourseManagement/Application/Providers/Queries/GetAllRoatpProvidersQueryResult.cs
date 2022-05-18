using System.Collections.Generic;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Models;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Models.ProviderRegistration;

namespace SFA.DAS.Roatp.CourseManagement.Application.Providers.Queries
{
    public class GetAllRoatpProvidersQueryResult
    {
        public List<ProviderRegistrationModel> Providers { get; set; }
    }
}