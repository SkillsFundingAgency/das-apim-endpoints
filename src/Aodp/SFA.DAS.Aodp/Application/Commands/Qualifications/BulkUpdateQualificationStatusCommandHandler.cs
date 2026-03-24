using MediatR;
using SFA.DAS.Aodp.Application.Commands.Qualifications;
using SFA.DAS.AODP.Domain.Qualifications.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Application.Commands.Qualifications;

public class BulkUpdateQualificationStatusCommandHandler : IRequestHandler<BulkUpdateQualificationStatusCommand, BaseMediatrResponse<BulkUpdateQualificationStatusCommandResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public BulkUpdateQualificationStatusCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BaseMediatrResponse<BulkUpdateQualificationStatusCommandResponse>> Handle(BulkUpdateQualificationStatusCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<BulkUpdateQualificationStatusCommandResponse>
        {
            Success = false
        };

        try
        {
            var apiResult = await _apiClient.PutWithResponseCode<BulkUpdateQualificationStatusCommandResponse>(
                new BulkUpdateQualificationStatusApiRequest(request));

            response.Success = true;
            response.Value = apiResult.Body;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ErrorMessage = ex.Message;
        }

        return response;
    }
}
