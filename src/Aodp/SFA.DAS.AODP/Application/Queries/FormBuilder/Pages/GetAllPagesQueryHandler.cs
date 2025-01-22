using AutoMapper;
using MediatR;
using SFA.DAS.AODP.Application.Queries.FormBuilder.Forms;
using SFA.DAS.AODP.Domain.FormBuilder.Requests.Pages;
using SFA.DAS.AODP.Domain.FormBuilder.Responses.Pages;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Application.FormBuilder.Pages.Queries;

public class GetAllPagesQueryHandler : IRequestHandler<GetAllPagesQuery, GetAllPagesQueryResponse>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;
    private readonly IMapper _mapper;

    public GetAllPagesQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient, IMapper mapper)
    {
        _apiClient = apiClient;
        _mapper = mapper;
    }

    public async Task<GetAllPagesQueryResponse> Handle(GetAllPagesQuery request, CancellationToken cancellationToken)
    {
        var response = new GetAllPagesQueryResponse();
        response.Success = false;
        try
        {
            var result = await _apiClient.Get<GetAllPagesApiResponse>(new GetAllPagesApiRequest(request.SectionId));
            response.Data = _mapper.Map<List<GetAllPagesQueryResponse.Page>>(result.Data);
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
        }

        return response;
    }
}