using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Questions;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Aodp.Application;
namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Questions;

public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, BaseMediatrResponse<EmptyResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


    public UpdateQuestionCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;

    }

    public async Task<BaseMediatrResponse<EmptyResponse>> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<EmptyResponse>()
        {
            Success = false
        };

        try
        {
            var apiRequest = new UpdateQuestionApiRequest(request.Id, request.PageId, request.FormVersionId, request.SectionId)
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
