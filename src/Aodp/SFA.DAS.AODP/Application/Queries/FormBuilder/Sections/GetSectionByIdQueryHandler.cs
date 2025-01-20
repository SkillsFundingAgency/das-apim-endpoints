using AutoMapper;
using MediatR;
using SFA.DAS.AODP.Api;
using SFA.DAS.AODP.Domain.FormBuilder.Requests.Sections;
using SFA.DAS.AODP.Domain.FormBuilder.Responses.Sections;

namespace SFA.DAS.AODP.Application.Queries.FormBuilder.Sections;

public class GetSectionByIdQueryHandler : IRequestHandler<GetSectionByIdQuery, GetSectionByIdQueryResponse>
{
    private readonly IApiClient _apiClient;
    private readonly IMapper _mapper;

    public GetSectionByIdQueryHandler(IApiClient apiClient, IMapper mapper)
    {
        _apiClient = apiClient;
        _mapper = mapper;
    }

    public async Task<GetSectionByIdQueryResponse> Handle(GetSectionByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new GetSectionByIdQueryResponse();
        response.Success = false;
        try
        {
            var result = await _apiClient.Get<GetSectionByIdApiResponse>(new GetSectionByIdApiRequest(request.SectionId, request.FormVersionId));
            _mapper.Map(result.Data, response.Data);
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
        }

        return response;
    }
}
