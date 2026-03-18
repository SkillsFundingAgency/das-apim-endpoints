using MediatR;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Application.Review
{
    public class GetFeedbackForApplicationReviewByIdQueryHandler : IRequestHandler<GetFeedbackForApplicationReviewByIdQuery, BaseMediatrResponse<GetFeedbackForApplicationReviewByIdQueryResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

        public GetFeedbackForApplicationReviewByIdQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<BaseMediatrResponse<GetFeedbackForApplicationReviewByIdQueryResponse>> Handle(GetFeedbackForApplicationReviewByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<GetFeedbackForApplicationReviewByIdQueryResponse>();
            response.Success = false;
            try
            {
                var result = await _apiClient.GetWithResponseCode<GetFeedbackForApplicationReviewByIdQueryResponse>(new GetFeedbackForApplicationReviewByIdApiRequest(request.ApplicationReviewId, request.UserType));

                result.EnsureSuccessStatusCode();
                response.Value = result.Body;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
    }


}

