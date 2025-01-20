using MediatR;
using SFA.DAS.AODP.Api;

namespace SFA.DAS.AODP.Application.Queries.FormBuilder.Sections;

public class GetSectionByIdQueryHandler : IRequestHandler<GetSectionByIdQuery, GetSectionByIdQueryResponse>
{
    private readonly IApiClient _apiClient;

    public GetSectionByIdQueryHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetSectionByIdQueryResponse> Handle(GetSectionByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new GetSectionByIdQueryResponse();
        response.Success = false;
        try
        {
            //response.Data = _sectionRepository.GetById(request.Id);
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
        }

        return response;
    }
}
