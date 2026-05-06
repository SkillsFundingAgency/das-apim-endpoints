using MediatR;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.Services;

public class EditApplicationCommandHandler : IRequestHandler<EditApplicationCommand, BaseMediatrResponse<EditApplicationCommandResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;
    public EditApplicationCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }


    public async Task<BaseMediatrResponse<EditApplicationCommandResponse>> Handle(EditApplicationCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<EditApplicationCommandResponse>
        {
            Success = false
        };

        try
        {
            var result = await _apiClient.PutWithResponseCode<EditApplicationCommandResponse>(new EditApplicationApiRequest()
            {
                ApplicationId = request.ApplicationId,
                Data = request
            });

            response.Success = true;
            response.Value.IsQanValid = result.Body.IsQanValid;
            response.Value.QanValidationMessage = result.Body.QanValidationMessage;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ErrorMessage = ex.Message;
        }

        return response;
    }
}
