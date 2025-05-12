using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

public class GetApplicationSectionByIdQueryHandler : IRequestHandler<GetApplicationSectionByIdQuery, BaseMediatrResponse<GetApplicationSectionByIdQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public GetApplicationSectionByIdQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BaseMediatrResponse<GetApplicationSectionByIdQueryResponse>> Handle(GetApplicationSectionByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetApplicationSectionByIdQueryResponse>();
        response.Success = false;
        try
        {
            var result = await _apiClient.Get<GetApplicationSectionByIdQueryResponse>(new GetApplicationSectionByIdApiRequest()
            {
                SectionId = request.SectionId,
                FormVersionId = request.FormVersionId,
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
