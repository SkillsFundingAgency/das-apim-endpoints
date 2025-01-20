using MediatR;
using SFA.DAS.AODP.Api;

namespace SFA.DAS.AODP.Application.FormBuilder.Pages.Queries;

public class GetAllPagesQueryHandler : IRequestHandler<GetAllPagesQuery, GetAllPagesQueryResponse>
{
    private readonly IApiClient _apiClient;
    public GetAllPagesQueryHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetAllPagesQueryResponse> Handle(GetAllPagesQuery request, CancellationToken cancellationToken)
    {
        var response = new GetAllPagesQueryResponse();
        response.Success = false;
        try
        {
            // response.Data = await _sectionRepository.GetSectionsForFormAsync(request.FormId);
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
        }

        return response;
    }
}