using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.Application.Applications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Application.Application;
public class GetApplicationFormAnswersByReviewIdQueryHandler : IRequestHandler<GetApplicationFormAnswersByReviewIdQuery, BaseMediatrResponse<GetApplicationFormAnswersByReviewIdQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;
    private readonly IMediator _mediator;

    public GetApplicationFormAnswersByReviewIdQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient, IMediator mediator)
    {
        _apiClient = apiClient;
        _mediator = mediator;
    }

    public async Task<BaseMediatrResponse<GetApplicationFormAnswersByReviewIdQueryResponse>> Handle(GetApplicationFormAnswersByReviewIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetApplicationFormAnswersByReviewIdQueryResponse>();
        response.Success = false;
        try
        {
            var answers = await _apiClient.Get<GetApplicationFormAnswersByReviewIdQueryResponse>(new GetApplicationFormAnswersByReviewIdApiRequest()
            {
                ApplicationReviewId = request.ApplicationReviewId,
            });

            response.Value = answers;
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
        }

        return response;
    }
}
