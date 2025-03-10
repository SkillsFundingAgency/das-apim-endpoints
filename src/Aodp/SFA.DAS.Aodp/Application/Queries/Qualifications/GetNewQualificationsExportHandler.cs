using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Qualifications
{
    public class GetNewQualificationsExportHandler : IRequestHandler<GetNewQualificationsExportQuery, BaseMediatrResponse<GetNewQualificationsExportResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

        public GetNewQualificationsExportHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<BaseMediatrResponse<GetNewQualificationsExportResponse>> Handle(GetNewQualificationsExportQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<GetNewQualificationsExportResponse>();
            try
            {
                var result = await _apiClient.Get<BaseMediatrResponse<GetNewQualificationsExportResponse>>(new GetNewQualificationCsvExportApiRequest());
                if (!result.Success || result?.Value == null)
                {
                    response.Success = false;
                    response.ErrorMessage = "No new qualifications found.";
                }
                else
                {
                    response.Value = new GetNewQualificationsExportResponse
                    {
                        QualificationExports = result.Value.QualificationExports
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
