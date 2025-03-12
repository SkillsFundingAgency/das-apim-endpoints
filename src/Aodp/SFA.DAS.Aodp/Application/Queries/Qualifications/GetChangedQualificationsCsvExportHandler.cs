using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Qualifications
{
    public class GetChangedQualificationsCsvExportHandler : IRequestHandler<GetChangedQualificationsCsvExportQuery, BaseMediatrResponse<GetChangedQualificationsCsvExportResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

        public GetChangedQualificationsCsvExportHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<BaseMediatrResponse<GetChangedQualificationsCsvExportResponse>> Handle(GetChangedQualificationsCsvExportQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<GetChangedQualificationsCsvExportResponse>();
            try
            {
                var result = await _apiClient.Get<BaseMediatrResponse<GetChangedQualificationsCsvExportResponse>>(new GetChangedQualificationCsvExportApiRequest());
                if (result == null)
                {
                    response.Success = false;
                    response.ErrorMessage = "No changed qualifications found.";
                }
                else if (result.Value.QualificationExports.Any())
                {
                    response.Value = new GetChangedQualificationsCsvExportResponse
                    {
                        QualificationExports = result.Value.QualificationExports
                    };
                    response.Success = true;
                }
                else
                {
                    response.Value = new GetChangedQualificationsCsvExportResponse
                    {
                        QualificationExports = new List<ChangedExport>()
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
