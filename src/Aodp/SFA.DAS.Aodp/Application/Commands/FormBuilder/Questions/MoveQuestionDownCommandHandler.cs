using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Questions;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Questions;

public class MoveQuestionDownCommandHandler : IRequestHandler<MoveQuestionDownCommand, BaseMediatrResponse<MoveQuestionDownCommandResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public MoveQuestionDownCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BaseMediatrResponse<MoveQuestionDownCommandResponse>> Handle(MoveQuestionDownCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<MoveQuestionDownCommandResponse>()
        {
            Success = false
        };

        try
        {
            await _apiClient.Put(new MoveQuestionDownApiRequest()
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
