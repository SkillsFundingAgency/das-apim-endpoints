using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.OutputFile;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Application.Queries.OutputFile;

public class GetPreviousOutputFilesQueryHandler : IRequestHandler<GetPreviousOutputFilesQuery, BaseMediatrResponse<GetPreviousOutputFilesQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


    public GetPreviousOutputFilesQueryHandler(IAodpApiClient<AodpApiConfiguration> aodpApiClient)
    {
        _apiClient = aodpApiClient;

    }

    public async Task<BaseMediatrResponse<GetPreviousOutputFilesQueryResponse>> Handle(GetPreviousOutputFilesQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetPreviousOutputFilesQueryResponse>
        {
            Success = false
        };

        try
        {
            var result = await _apiClient.Get<GetPreviousOutputFilesQueryResponse>(new GetPreviousOutputFilesApiRequest());
            response.Value = result;
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