using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Sections;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.FormBuilder.Sections;

public class GetAllSectionsQueryHandler : IRequestHandler<GetAllSectionsQuery, BaseMediatrResponse<GetAllSectionsQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


    public GetAllSectionsQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;

    }

    public async Task<BaseMediatrResponse<GetAllSectionsQueryResponse>> Handle(GetAllSectionsQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetAllSectionsQueryResponse>();
        response.Success = false;
        try
        {
            var result = await _apiClient.Get<GetAllSectionsQueryResponse>(new GetAllSectionsApiRequest()
            {
                FormVersionId = request.FormVersionId,
            });
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