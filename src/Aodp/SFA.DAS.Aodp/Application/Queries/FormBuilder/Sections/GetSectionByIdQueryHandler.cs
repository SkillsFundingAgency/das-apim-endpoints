using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Sections;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.FormBuilder.Sections;

public class GetSectionByIdQueryHandler : IRequestHandler<GetSectionByIdQuery, BaseMediatrResponse<GetSectionByIdQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


    public GetSectionByIdQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;

    }

    public async Task<BaseMediatrResponse<GetSectionByIdQueryResponse>> Handle(GetSectionByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetSectionByIdQueryResponse>();
        response.Success = false;
        try
        {
            var result = await _apiClient.Get<GetSectionByIdQueryResponse>(new GetSectionByIdApiRequest(request.SectionId, request.FormVersionId));
            response.Value = result;
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
        }

        return response;
    }
}
