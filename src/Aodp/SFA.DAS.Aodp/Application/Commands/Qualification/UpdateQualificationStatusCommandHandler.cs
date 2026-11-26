using MediatR;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.AODP.Domain.Qualifications.Requests;
using SFA.DAS.Aodp.Services;

namespace SFA.DAS.AODP.Application.Commands.Qualification;

public class UpdateQualificationStatusCommandHandler : IRequestHandler<UpdateQualificationStatusCommand, BaseMediatrResponse<EmptyResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public UpdateQualificationStatusCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BaseMediatrResponse<EmptyResponse>> Handle(UpdateQualificationStatusCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<EmptyResponse>
        {
            Success = false
        };

        try
        {
            await _apiClient.PostWithResponseCode<EmptyResponse>(new UpdateQualificationStatusApiRequest(request));

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
