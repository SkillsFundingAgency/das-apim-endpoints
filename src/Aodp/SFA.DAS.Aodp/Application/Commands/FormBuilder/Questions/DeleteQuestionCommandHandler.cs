using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Questions;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Questions;
public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, BaseMediatrResponse<DeleteQuestionCommandResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;
    public DeleteQuestionCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }
    public async Task<BaseMediatrResponse<DeleteQuestionCommandResponse>> Handle(DeleteQuestionCommand command, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<DeleteQuestionCommandResponse>()
        {
            Success = false
        };
        try
        {
            var apiRequest = new DeleteQuestionApiRequest(command.QuestionId, command.PageId, command.FormVersionId, command.SectionId)
            {
                Data = command
            };
            await _apiClient.Delete(apiRequest);
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