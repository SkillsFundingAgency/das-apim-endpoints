﻿using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, BaseMediatrResponse<CreateApplicationCommandResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;
    public CreateApplicationCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }


    public async Task<BaseMediatrResponse<CreateApplicationCommandResponse>> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<CreateApplicationCommandResponse>
        {
            Success = false
        };

        try
        {
            var result = await _apiClient.PostWithResponseCode<CreateApplicationCommandResponse>(new CreateApplicationApiRequest()
            {
                Data = request
            });

            response.Value.Id = result.Body.Id;
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
