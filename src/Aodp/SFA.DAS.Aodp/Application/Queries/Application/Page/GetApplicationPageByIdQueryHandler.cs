using MediatR;
using SFA.DAS.Aodp.Application;
using SFA.DAS.SharedOuterApi.Configuration;using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Application.Page;

public class GetApplicationPageByIdQueryHandler : IRequestHandler<GetApplicationPageByIdQuery, BaseMediatrResponse<GetApplicationPageByIdQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


    public GetApplicationPageByIdQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;

    }

    public async Task<BaseMediatrResponse<GetApplicationPageByIdQueryResponse>> Handle(GetApplicationPageByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetApplicationPageByIdQueryResponse>();
        response.Success = false;
        try
        {
            var result = await _apiClient.Get<GetApplicationPageByIdQueryResponse>(new GetApplicationPageByIdApiRequest()
            {
                FormVersionId = request.FormVersionId,
                SectionId = request.SectionId,
                PageOrder = request.PageOrder
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
