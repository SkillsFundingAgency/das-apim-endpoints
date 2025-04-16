using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.OutputFile;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Application.Commands.OutputFile;

public class GenerateNewOutputFileCommandHandler : IRequestHandler<GenerateNewOutputFileCommand, BaseMediatrResponse<EmptyResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


    public GenerateNewOutputFileCommandHandler(IAodpApiClient<AodpApiConfiguration> aodpApiClient)
    {
        _apiClient = aodpApiClient;

    }

    public async Task<BaseMediatrResponse<EmptyResponse>> Handle(GenerateNewOutputFileCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<EmptyResponse>
        {
            Success = false
        };

        try
        {
            var result = await _apiClient.PostWithResponseCode<EmptyResponse>(new GenerateNewOutputFileApiRequest()
            {
                Data = request
            });
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
            response.Success = false;
        }

        return response;
    }
}