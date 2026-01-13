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
            try
            {
                var result = await _apiClient.Get<BaseMediatrResponse<GetQualificationOutputFileResponse>>(
                    new GetQualificationOutputFileApiRequest(request.CurrentUsername));

                if (result == null)
                {
                    return new BaseMediatrResponse<GetQualificationOutputFileResponse>
                    {
                        Success = false,
                        ErrorMessage = "No response received from inner API.",
                        ErrorCode = ErrorCodes.UnexpectedError
                    };
                }

                if (result.Value?.ZipFileContent == null || result.Value.ZipFileContent.Length == 0)
                {
                    return new BaseMediatrResponse<GetQualificationOutputFileResponse>
                    {

                        Success = false,
                        ErrorMessage = result.ErrorMessage ?? "Export file not returned.",
                        ErrorCode = result.ErrorCode ?? ErrorCodes.NoData

                    };
                }

                return new BaseMediatrResponse<GetQualificationOutputFileResponse>
                {
                    Success = true,
                    Value = new GetQualificationOutputFileResponse
                    {
                        FileName = result.Value.FileName,
                        ZipFileContent = result.Value.ZipFileContent,
                        ContentType = result.Value.ContentType
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseMediatrResponse<GetQualificationOutputFileResponse>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    ErrorCode = ErrorCodes.UnexpectedError
                };
            }

        }
    }
}
