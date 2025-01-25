using MediatR;
using SFA.DAS.AODP.Domain.FormBuilder.Requests.Sections;
using SFA.DAS.AODP.Domain.FormBuilder.Responses.Sections;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Application.Queries.FormBuilder.Sections;

public class GetSectionByIdQueryHandler : IRequestHandler<GetSectionByIdQuery, GetSectionByIdQueryResponse>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;
    

    public GetSectionByIdQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
       
    }

    public async Task<GetSectionByIdQueryResponse> Handle(GetSectionByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new GetSectionByIdQueryResponse();
        response.Success = false;
        try
        {
            var result = await _apiClient.Get<GetSectionByIdApiResponse>(new GetSectionByIdApiRequest(request.SectionId, request.FormVersionId));
            response.Data = result.Data;
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
        }

        return response;
    }
}
