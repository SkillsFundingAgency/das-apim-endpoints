using MediatR;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.Services;

public class GetApplicationPageAnswersByPageIdQueryHandler : IRequestHandler<GetApplicationPageAnswersByPageIdQuery, BaseMediatrResponse<GetApplicationPageAnswersByPageIdQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


    public GetApplicationPageAnswersByPageIdQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;

    }

    public async Task<BaseMediatrResponse<GetApplicationPageAnswersByPageIdQueryResponse>> Handle(GetApplicationPageAnswersByPageIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetApplicationPageAnswersByPageIdQueryResponse>();
        response.Success = false;
        try
        {
            var result = await _apiClient.Get<GetApplicationPageAnswersByPageIdQueryResponse>(new GetApplicationPageAnswersByPageIdApiRequest()
            {
                FormVersionId = request.FormVersionId,
                SectionId = request.SectionId,
                PageId = request.PageId,
                ApplicationId = request.ApplicationId,
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
