using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Qualifications
{
    public class GetQualificationVersionsForQualificationByReferenceQueryHandler : IRequestHandler<GetQualificationVersionsForQualificationByReferenceQuery, BaseMediatrResponse<GetQualificationVersionsForQualificationByReferenceQueryResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

        public GetQualificationVersionsForQualificationByReferenceQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<BaseMediatrResponse<GetQualificationVersionsForQualificationByReferenceQueryResponse>> Handle(GetQualificationVersionsForQualificationByReferenceQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<GetQualificationVersionsForQualificationByReferenceQueryResponse>();
            response.Success = false;
            try
            {
                var result = await _apiClient.GetWithResponseCode<GetQualificationVersionsForQualificationByReferenceQueryResponse>(new GetQualificationVersionsForQualificationByReferenceApiRequest(request.QualificationReference));

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

