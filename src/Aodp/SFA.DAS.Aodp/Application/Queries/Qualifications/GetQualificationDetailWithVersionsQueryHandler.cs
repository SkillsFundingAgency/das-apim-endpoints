using MediatR;
using SFA.DAS.AODP.Application.Queries.Qualifications;
using SFA.DAS.AODP.Domain.Qualifications.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Qualifications
{

    public class GetQualificationDetailWithVersionsQueryHandler : IRequestHandler<GetQualificationDetailWithVersionsQuery, BaseMediatrResponse<GetQualificationDetailsQueryResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

        public GetQualificationDetailWithVersionsQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<BaseMediatrResponse<GetQualificationDetailsQueryResponse>> Handle(GetQualificationDetailWithVersionsQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<GetQualificationDetailsQueryResponse>();
            response.Success = false;
            try
            {
                var result = await _apiClient.Get<GetQualificationDetailsQueryResponse>(new GetQualificationDetailWithVersionsApiRequest(request.QualificationReference));
                if (result != null)
                {
                    response.Value = result;
                    response.Success = true;
                }
                else
                {
                    response.Success = false;
                    response.ErrorMessage = $"No details found for qualification reference: {request.QualificationReference}";
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
    }
}
