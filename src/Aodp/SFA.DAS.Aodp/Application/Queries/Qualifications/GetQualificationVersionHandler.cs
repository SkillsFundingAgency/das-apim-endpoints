using MediatR;
using SFA.DAS.Aodp.Application.Queries.Qualifications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Application.Queries.Qualifications
{
    public class GetQualificationVersionHandler : IRequestHandler<GetQualificationVersionQuery, BaseMediatrResponse<GetQualificationDetailsQueryResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

        public GetQualificationVersionHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<BaseMediatrResponse<GetQualificationDetailsQueryResponse>> Handle(GetQualificationVersionQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<GetQualificationDetailsQueryResponse>();
            response.Success = false;
            try
            {
                var result = await _apiClient.GetWithResponseCode<GetQualificationDetailsQueryResponse>(new GetQualificationVersionApiRequest(request.QualificationReference,request.Version));

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


