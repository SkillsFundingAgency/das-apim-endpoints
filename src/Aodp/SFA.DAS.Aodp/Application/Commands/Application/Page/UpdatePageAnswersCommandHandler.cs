using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

public class UpdatePageAnswersCommandHandler : IRequestHandler<UpdatePageAnswersCommand, BaseMediatrResponse<UpdatePageAnswersCommandResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


    public UpdatePageAnswersCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BaseMediatrResponse<UpdatePageAnswersCommandResponse>> Handle(UpdatePageAnswersCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<UpdatePageAnswersCommandResponse>()
        {
            Success = false
        };

        try
        {
            var apiRequest = new UpdatePageAnswersApiRequest(request.ApplicationId, request.PageId, request.FormVersionId, request.SectionId)
            {
                Data = request
            };
            await _apiClient.Put(apiRequest);
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
            response.Success = false;
        }

        return response;
    }
}
