using MediatR;
using SFA.DAS.Aodp.Domain.FormBuilder.Requests.Pages;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.FormBuilder.Pages;

public class GetAllPagesQueryHandler : IRequestHandler<GetAllPagesQuery, BaseMediatrResponse<GetAllPagesQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


    public GetAllPagesQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;

    }

    public async Task<BaseMediatrResponse<GetAllPagesQueryResponse>> Handle(GetAllPagesQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetAllPagesQueryResponse>();
        response.Success = false;
        try
        {
            var result = await _apiClient.Get<GetAllPagesQueryResponse>(new GetAllPagesApiRequest()
            {
                FormVersionId = request.FormVersionId,
                SectionId = request.SectionId
            });
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