using MediatR;
using Microsoft.AspNetCore.Http;
using SFA.DAS.Aodp.InnerApi.AodpApi.Import;
using SFA.DAS.Aodp.Wrapper;

namespace SFA.DAS.Aodp.Application.Commands.Import;

public class ImportPldnsCommandHandler : IRequestHandler<ImportPldnsCommand, BaseMediatrResponse<ImportPldnsCommandResponse>>
{
    private readonly IMultipartFormDataSenderWrapper _multipartFormDataSenderWrapper;

    public ImportPldnsCommandHandler(IMultipartFormDataSenderWrapper multipartFormDataSenderWrapper)
    {
        _multipartFormDataSenderWrapper = multipartFormDataSenderWrapper;
    }
    public async Task<BaseMediatrResponse<ImportPldnsCommandResponse>> Handle(ImportPldnsCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<ImportPldnsCommandResponse>
        {
            Success = false
        };

        try
        {
            var apiRequest = new ImportPldnsApiRequest
            {
                Data = request.File!
            };

            var importResponse = await _multipartFormDataSenderWrapper
                        .PostWithMultipartFormData<IFormFile, ImportPldnsCommandResponse>(apiRequest, true, cancellationToken);

            response.Value = importResponse.Body;
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ErrorMessage = ex.Message;
        }

        return response;
    }
}
