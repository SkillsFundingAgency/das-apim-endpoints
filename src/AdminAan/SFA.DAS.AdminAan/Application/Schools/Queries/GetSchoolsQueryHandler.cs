using MediatR;
using SFA.DAS.AdminAan.Infrastructure;

namespace SFA.DAS.AdminAan.Application.Schools.Queries;
public class GetSchoolsQueryHandler : IRequestHandler<GetSchoolsQuery, GetSchoolsQueryApiResult>
{
    private readonly IReferenceDataApiClient _apiClient;

    public const int PageSize = 100;
    public const int PageNumber = 1;
    public GetSchoolsQueryHandler(IReferenceDataApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetSchoolsQueryApiResult> Handle(GetSchoolsQuery request, CancellationToken cancellationToken)
    {
        var result = await _apiClient.GetSchools(request.SearchTerm, PageSize, PageNumber);

        return result.ResponseMessage.StatusCode == System.Net.HttpStatusCode.OK
            ? result.GetContent()
            : new GetSchoolsQueryApiResult(new List<School>());
    }
}
