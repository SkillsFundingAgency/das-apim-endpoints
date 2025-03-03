using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Pages;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.FormBuilder.Pages;

public class GetPageByIdQueryHandler : IRequestHandler<GetPageByIdQuery, BaseMediatrResponse<GetPageByIdQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


    public GetPageByIdQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;

    }

    public async Task<BaseMediatrResponse<GetPageByIdQueryResponse>> Handle(GetPageByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetPageByIdQueryResponse>();
        response.Success = false;
        try
        {
            var result = await _apiClient.Get<GetPageByIdQueryResponse>(new GetPageByIdApiRequest()
            {
                FormVersionId = request.FormVersionId,
                SectionId = request.SectionId,
                PageId = request.PageId
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
