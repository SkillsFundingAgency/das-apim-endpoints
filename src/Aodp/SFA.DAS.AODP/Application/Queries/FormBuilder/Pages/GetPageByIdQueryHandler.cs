using MediatR;
using SFA.DAS.AODP.Api;

namespace SFA.DAS.AODP.Application.FormBuilder.Pages.Queries;

public class GetPageByIdQueryHandler : IRequestHandler<GetPageByIdQuery, GetPageByIdQueryResponse>
{
    private readonly IApiClient _apiClient;

    public GetPageByIdQueryHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetPageByIdQueryResponse> Handle(GetPageByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new GetPageByIdQueryResponse();
        response.Success = false;
        try
        {
            //response.Data = _pageRepository.GetById(request.Id);
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
        }

        return response;
    }
}
