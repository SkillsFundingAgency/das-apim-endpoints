using System.Collections.Generic;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Models;

namespace SFA.DAS.Roatp.CourseManagement.Application.Providers.Queries
{
    public class GetAllRoatpProvidersQueryResult
    {
        public List<RoatpProviderModel> Providers { get; set; }
    }
}