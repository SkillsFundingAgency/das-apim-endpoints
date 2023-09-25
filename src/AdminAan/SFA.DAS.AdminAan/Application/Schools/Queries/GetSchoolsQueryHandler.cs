using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.AdminAan.Infrastructure;

namespace SFA.DAS.AdminAan.Application.Schools.Queries;
public class GetSchoolsQueryHandler : IRequestHandler<GetSchoolsQuery, GetSchoolsQueryApiResult>
{
    private readonly IReferenceDataApiClient _apiClient;
    private readonly ILogger<GetSchoolsQueryHandler> _logger;

    public const int PageSize = 20;
    public const int PageNumber = 1;
    public GetSchoolsQueryHandler(IReferenceDataApiClient apiClient, ILogger<GetSchoolsQueryHandler> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    public async Task<GetSchoolsQueryApiResult> Handle(GetSchoolsQuery request, CancellationToken cancellationToken)
    {
        var result = await _apiClient.GetSchools(request.SearchTerm, PageSize, PageNumber, cancellationToken);

        if (result.ResponseMessage.StatusCode == System.Net.HttpStatusCode.OK) return result.GetContent();

        _logger.LogInformation("Call to ReferenceData Api was not successful. There has been status returned of StatusCode {StatusCode}", result.ResponseMessage.StatusCode);

        return new GetSchoolsQueryApiResult(new List<School>());
    }
}
