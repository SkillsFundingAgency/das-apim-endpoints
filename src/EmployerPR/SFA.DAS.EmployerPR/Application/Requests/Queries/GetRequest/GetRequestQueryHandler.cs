using MediatR;
using RestEase;
using SFA.DAS.EmployerPR.Infrastructure;
using SFA.DAS.EmployerPR.InnerApi.Responses;

namespace SFA.DAS.EmployerPR.Application.Requests.Queries.GetRequest;

public class GetRequestQueryHandler(IProviderRelationshipsApiRestClient _providerRelationshipsApiRestClient) : IRequestHandler<GetRequestQuery, GetRequestResponse?>
{
    public async Task<GetRequestResponse?> Handle(GetRequestQuery request, CancellationToken cancellationToken)
    {
        Response<GetRequestResponse?> response = await _providerRelationshipsApiRestClient.GetRequest(request.RequestId, cancellationToken);

        return response.ResponseMessage.IsSuccessStatusCode ? response.GetContent() : null;
    }
}
