using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Questions;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Questions;

public class MoveQuestionUpCommandHandler : IRequestHandler<MoveQuestionUpCommand, BaseMediatrResponse<MoveQuestionUpCommandResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public MoveQuestionUpCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BaseMediatrResponse<MoveQuestionUpCommandResponse>> Handle(MoveQuestionUpCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<MoveQuestionUpCommandResponse>()
        {
            Success = false
        };

        try
        {
            await _apiClient.Put(new MoveQuestionUpApiRequest()
            {
                QuestionId = request.QuestionId,
                FormVersionId = request.FormVersionId,
                PageId = request.PageId,
                SectionId = request.SectionId
            });
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ErrorMessage = ex.Message;
        }

        return response;
    }
}
