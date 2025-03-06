using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Questions;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Questions;

public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, BaseMediatrResponse<CreateQuestionCommandResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


    public CreateQuestionCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;

    }

    public async Task<BaseMediatrResponse<CreateQuestionCommandResponse>> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<CreateQuestionCommandResponse>();
        try
        {
            var apiRequestData = new CreateQuestionApiRequest()
            {
                Data = request,
                SectionId = request.SectionId,
                FormVersionId = request.FormVersionId,
                PageId = request.PageId
            };

            var result = await _apiClient.PostWithResponseCode<CreateQuestionCommandResponse>(apiRequestData);
            response.Value.Id = result!.Body.Id;
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
