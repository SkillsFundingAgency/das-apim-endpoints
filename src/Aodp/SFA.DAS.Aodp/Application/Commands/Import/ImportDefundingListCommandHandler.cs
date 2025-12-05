using MediatR;
using Microsoft.AspNetCore.Http;
using SFA.DAS.Aodp.InnerApi.AodpApi.Import;
using SFA.DAS.Aodp.Wrapper;

namespace SFA.DAS.Aodp.Application.Commands.Import;

public class ImportDefundingListCommandHandler : IRequestHandler<ImportDefundingListCommand, BaseMediatrResponse<ImportDefundingListResponse>>
{
    private readonly IMultipartFormDataSenderWrapper _multipartFormDataSenderWrapper;

    public ImportDefundingListCommandHandler(IMultipartFormDataSenderWrapper multipartFormDataSenderWrapper)
    {
        _multipartFormDataSenderWrapper = multipartFormDataSenderWrapper;
    }

    public async Task<BaseMediatrResponse<ImportDefundingListResponse>> Handle(ImportDefundingListCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<ImportDefundingListResponse>
        {
            Success = false
        };

        try
        {
            var apiRequest = new ImportDefundingListApiRequest
            {
                Data = request.File!
            };

            var importResponse = await _multipartFormDataSenderWrapper
                        .PostWithMultipartFormData<IFormFile, ImportDefundingListResponse>(apiRequest, true, cancellationToken);

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