using System;
using System.Threading.Tasks;
using SFA.DAS.Approvals.InnerApi.CoursesApi;

namespace SFA.DAS.Approvals.Services;

public interface IApprenticeshipUnitVersionService
{
    Task<GetCourseLookupResponse> Resolve(GetCourseLookupResponse course, DateTime? actualStartDate);
    Task<GetCourseLookupResponse> Resolve(string courseCode, DateTime? actualStartDate);
}
