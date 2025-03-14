using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Application.Review
{
    public class GetApplicationForReviewByIdQueryHandler : IRequestHandler<GetApplicationForReviewByIdQuery, BaseMediatrResponse<GetApplicationForReviewByIdQueryResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

        public GetApplicationForReviewByIdQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<BaseMediatrResponse<GetApplicationForReviewByIdQueryResponse>> Handle(GetApplicationForReviewByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<GetApplicationForReviewByIdQueryResponse>();
            response.Success = false;
            try
            {
                var result = await _apiClient.GetWithResponseCode<GetApplicationForReviewByIdQueryResponse>(new GetApplicationReviewByIdApiRequest(request.ApplicationReviewId));

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

