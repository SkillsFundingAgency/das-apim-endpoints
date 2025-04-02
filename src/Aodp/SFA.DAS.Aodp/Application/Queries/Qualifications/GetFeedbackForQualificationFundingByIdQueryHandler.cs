using MediatR;
using SFA.DAS.Aodp.Application.Queries.Qualifications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Application.Review
{
    public class GetFeedbackForQualificationFundingByIdQueryHandler : IRequestHandler<GetFeedbackForQualificationFundingByIdQuery, BaseMediatrResponse<GetFeedbackForQualificationFundingByIdQueryResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

        public GetFeedbackForQualificationFundingByIdQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<BaseMediatrResponse<GetFeedbackForQualificationFundingByIdQueryResponse>> Handle(GetFeedbackForQualificationFundingByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<GetFeedbackForQualificationFundingByIdQueryResponse>();
            response.Success = false;
            try
            {
                var result = await _apiClient.GetWithResponseCode<GetFeedbackForQualificationFundingByIdQueryResponse>(new GetFeedbackForQualificationFundingByIdApiRequest(request.QualificationVersionId));

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

