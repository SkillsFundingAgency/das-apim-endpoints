using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Qualifications
{
    public class GetNewQualificationsExportHandler : IRequestHandler<GetNewQualificationsExportQuery, BaseMediatrResponse<GetQualificationsExportResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

        public GetNewQualificationsExportHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<BaseMediatrResponse<GetQualificationsExportResponse>> Handle(GetNewQualificationsExportQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<GetQualificationsExportResponse>();
            try
            {
                var result = await _apiClient.Get<BaseMediatrResponse<GetQualificationsExportResponse>>(new GetNewQualificationCsvExportApiRequest());
                if (!result.Success || result?.Value == null)
                {
                    response.Success = false;
                    response.ErrorMessage = "No new qualifications found.";
                }
                else
                {
                    response.Value = new GetQualificationsExportResponse
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
