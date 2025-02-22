using MediatR;
using SFA.DAS.Aodp.Application;
using SFA.DAS.SharedOuterApi.Configuration;using SFA.DAS.SharedOuterApi.Interfaces;

public class GetApplicationsByOrganisationIdQueryHandler : IRequestHandler<GetApplicationsByOrganisationIdQuery, BaseMediatrResponse<GetApplicationsByOrganisationIdQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public GetApplicationsByOrganisationIdQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BaseMediatrResponse<GetApplicationsByOrganisationIdQueryResponse>> Handle(GetApplicationsByOrganisationIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetApplicationsByOrganisationIdQueryResponse>();
        response.Success = false;
        try
        {
            var result = await _apiClient.Get<GetApplicationsByOrganisationIdQueryResponse>(new GetApplicationsByOrganisationIdApiRequest());
            response.Value = result;
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
        }

        return response;
    }
}
