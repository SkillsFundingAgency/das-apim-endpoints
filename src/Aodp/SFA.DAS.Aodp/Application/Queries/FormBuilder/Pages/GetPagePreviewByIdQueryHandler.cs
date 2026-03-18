using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Pages;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.FormBuilder.Pages;

public class GetPagePreviewByIdQueryHandler : IRequestHandler<GetPagePreviewByIdQuery, BaseMediatrResponse<GetPagePreviewByIdQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


    public GetPagePreviewByIdQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;

    }

    public async Task<BaseMediatrResponse<GetPagePreviewByIdQueryResponse>> Handle(GetPagePreviewByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetPagePreviewByIdQueryResponse>();
        response.Success = false;
        try
        {
            var result = await _apiClient.Get<GetPagePreviewByIdQueryResponse>(new GetPagePreviewByIdApiRequest()
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