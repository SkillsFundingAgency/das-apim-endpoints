using MediatR;
using RestEase;
using SFA.DAS.AdminAan.Infrastructure;

namespace SFA.DAS.AdminAan.Application.Schools.Queries;
public class GetSchoolsQueryHandler : IRequestHandler<GetSchoolsQuery, Response<GetSchoolsQueryResult>>
{
    private readonly IReferenceDataApiClient _apiClient;

    public GetSchoolsQueryHandler(IReferenceDataApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<Response<GetSchoolsQueryResult>> Handle(GetSchoolsQuery request, CancellationToken cancellationToken)
    {
        return await _apiClient.GetSchools(request.SearchTerm);
    }
}
