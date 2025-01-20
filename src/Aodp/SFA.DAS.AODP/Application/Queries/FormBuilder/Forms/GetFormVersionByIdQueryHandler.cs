﻿using AutoMapper;
using MediatR;
using SFA.DAS.AODP.Api;
using SFA.DAS.AODP.Domain.FormBuilder.Requests.Forms;
using SFA.DAS.AODP.Domain.FormBuilder.Responses.Forms;

namespace SFA.DAS.AODP.Application.Queries.FormBuilder.Forms;

public class GetFormVersionByIdQueryHandler : IRequestHandler<GetFormVersionByIdQuery, GetFormVersionByIdQueryResponse>
{
    private readonly IApiClient _apiClient;
    private readonly IMapper _mapper;

    public GetFormVersionByIdQueryHandler(IApiClient apiClient, IMapper mapper)
    {
        _apiClient = apiClient;
        _mapper = mapper;
    }

    public async Task<GetFormVersionByIdQueryResponse> Handle(GetFormVersionByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new GetFormVersionByIdQueryResponse();
        response.Success = false;
        try
        {
            //var formVersion = await _formRepository.GetFormVersionByIdAsync(request.Id);
            //response.Data = _mapper.Map<FormVersion?, ViewModels.FormVersion?>(formVersion);
            var result = await _apiClient.Get<GetFormVersionByIdApiResponse>(new GetFormVersionByIdApiRequest(request.FormVersionId));
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
