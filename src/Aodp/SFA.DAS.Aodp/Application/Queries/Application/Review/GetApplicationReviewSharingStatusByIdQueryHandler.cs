using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Application.Review
{
    public class GetApplicationReviewSharingStatusByIdQueryHandler : IRequestHandler<GetApplicationReviewSharingStatusByIdQuery, BaseMediatrResponse<GetApplicationReviewSharingStatusByIdQueryResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

        public GetApplicationReviewSharingStatusByIdQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<BaseMediatrResponse<GetApplicationReviewSharingStatusByIdQueryResponse>> Handle(GetApplicationReviewSharingStatusByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<GetApplicationReviewSharingStatusByIdQueryResponse>();
            response.Success = false;
            try
            {
                var result = await _apiClient.GetWithResponseCode<GetApplicationReviewSharingStatusByIdQueryResponse>(new GetApplicationReviewSharingStatusByIdApiRequest(request.ApplicationReviewId));

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

