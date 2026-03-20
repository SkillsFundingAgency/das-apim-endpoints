using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications;
using SFA.DAS.AODP.Domain.Qualifications.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Qualifications
{
    public class GetQualificationOutputFileLogQueryHandler : IRequestHandler<GetQualificationOutputFileLogQuery, BaseMediatrResponse<GetQualificationOutputFileLogResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

        public GetQualificationOutputFileLogQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<BaseMediatrResponse<GetQualificationOutputFileLogResponse>> Handle(GetQualificationOutputFileLogQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<GetQualificationOutputFileLogResponse>();
            try
            {
                var result = await _apiClient.Get<BaseMediatrResponse<GetQualificationOutputFileLogResponse>>(new GetQualificationOutputFileLogApiRequest());
                if (!result.Success || result?.Value == null)
                {
                    response.Success = false;
                    response.ErrorMessage = "No output file logs found.";
                }
                else
                {
                    response.Value = new GetQualificationOutputFileLogResponse
                    {
                        OutputFileLogs = result.Value.OutputFileLogs
                    };
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
    }
}
