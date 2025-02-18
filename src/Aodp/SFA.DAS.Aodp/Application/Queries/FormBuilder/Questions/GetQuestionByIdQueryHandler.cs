using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Questions;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.FormBuilder.Questions;

public class GetQuestionByIdQueryHandler : IRequestHandler<GetQuestionByIdQuery, BaseMediatrResponse<GetQuestionByIdQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


    public GetQuestionByIdQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;

    }

    public async Task<BaseMediatrResponse<GetQuestionByIdQueryResponse>> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetQuestionByIdQueryResponse>();
        response.Success = false;
        try
        {
            var result = await _apiClient.Get<GetQuestionByIdQueryResponse>(new GetQuestionByIdApiRequest()
            {
                FormVersionId = request.FormVersionId,
                SectionId = request.SectionId,
                PageId = request.PageId,
                QuestionId = request.QuestionId
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