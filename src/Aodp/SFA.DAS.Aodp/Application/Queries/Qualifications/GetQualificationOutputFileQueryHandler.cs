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
        public async Task<BaseMediatrResponse<GetQualificationOutputFileResponse>> Handle(
    GetQualificationOutputFileQuery request,
    CancellationToken cancellationToken)
        {
            try
            {
                var result = await _apiClient.PostWithResponseCode<BaseMediatrResponse<GetQualificationOutputFileResponse>>(
                    new GetQualificationOutputFileApiRequest(request));

                var inner = result?.Body;

                if (inner == null)
                {
                    return new BaseMediatrResponse<GetQualificationOutputFileResponse>
                    {
                        Success = false,
                        ErrorMessage = "No response from inner API.",
                        ErrorCode = ErrorCodes.UnexpectedError
                    };
                }

                if (!inner.Success)
                {
                    return new BaseMediatrResponse<GetQualificationOutputFileResponse>
                    {
                        Success = false,
                        ErrorMessage = inner.ErrorMessage ?? "Inner API failed.",
                        ErrorCode = inner.ErrorCode ?? ErrorCodes.UnexpectedError
                    };
                }

                if (inner.Value?.ZipFileContent == null || inner.Value.ZipFileContent.Length == 0)
                {
                    return new BaseMediatrResponse<GetQualificationOutputFileResponse>
                    {
                        Success = false,
                        ErrorMessage = "Export file not returned.",
                        ErrorCode = ErrorCodes.NoData
                    };
                }

                return new BaseMediatrResponse<GetQualificationOutputFileResponse>
                {
                    Success = true,
                    Value = inner.Value
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
