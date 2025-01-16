using MediatR;
using SFA.DAS.AODP.InnerApi.AodpApi.Request;
using SFA.DAS.AODP.InnerApi.AodpApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Application.Queries.FormBuilder.Form;

public class GetAllFormsQueryHandler : IRequestHandler<GetAllFormsQuery, GetAllFormsQueryResponse>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _aodpApiClient;

    public GetAllFormsQueryHandler(IAodpApiClient<AodpApiConfiguration> aodpApiClient)
    {
        _aodpApiClient = aodpApiClient;
    }

    public async Task<GetAllFormsQueryResponse> Handle(GetAllFormsQuery request, CancellationToken cancellationToken)
    {
        var queryResponse = new GetAllFormsQueryResponse
        {
            Success = false
        };
        try
        {
            var data = await _aodpApiClient.Get<GetAllFormsResponse>(new GetAllFormsRequest());

            queryResponse.Forms = new();
            foreach (var form in data.Forms)
            {
                queryResponse.Forms.Add(new()
                {
                    Id = form.Id,
                    Name = form.Name,
                    Description = form.Description,
                    Version = form.Version

                });
            }

            queryResponse.Success = true;
        }
        catch (Exception ex)
        {
            queryResponse.ErrorMessage = ex.Message;
        }

        return queryResponse;
    }
}