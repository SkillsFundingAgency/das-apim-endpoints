using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Qualifications
{
    public class GetQualificationOutputFileQueryHandler : IRequestHandler<GetQualificationOutputFileQuery, BaseMediatrResponse<GetQualificationOutputFileResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

        public GetQualificationOutputFileQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<BaseMediatrResponse<GetQualificationOutputFileResponse>> Handle(GetQualificationOutputFileQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<GetQualificationOutputFileResponse>();
            try
            {
                var result = await _apiClient.Get<BaseMediatrResponse<GetQualificationOutputFileResponse>>(
                    new GetQualificationOutputFileApiRequest());

                if (!result.Success || result?.Value == null ||
                    result.Value.ZipFileContent == null || result.Value.ZipFileContent.Length == 0)
                {
                    response.Success = false;
                    response.ErrorMessage = result?.ErrorMessage ?? "Export file not available.";
                }
                else
                {
                    response.Value = new GetQualificationOutputFileResponse
                    {
                        FileName = result.Value.FileName,
                        ZipFileContent = result.Value.ZipFileContent,
                        ContentType = result.Value.ContentType
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
