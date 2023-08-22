using RestEase;
using SFA.DAS.AdminAan.Application.Schools.Queries;

namespace SFA.DAS.AdminAan.Infrastructure;

public interface IReferenceDataApiClient
{
    [Get("api/organisations/educational")]
    [AllowAnyStatusCode]
    Task<Response<GetSchoolsQueryResult>> GetSchools(string searchTerm, int pageSize = 100, int pageNumber = 1);
}