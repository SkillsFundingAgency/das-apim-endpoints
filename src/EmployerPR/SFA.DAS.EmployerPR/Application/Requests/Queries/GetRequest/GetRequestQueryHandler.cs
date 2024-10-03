using MediatR;
using RestEase;
using SFA.DAS.EmployerPR.Infrastructure;

namespace SFA.DAS.EmployerPR.Application.Requests.Queries.GetRequest;

public class GetRequestQueryHandler(IProviderRelationshipsApiRestClient _providerRelationshipsApiRestClient) : IRequestHandler<GetRequestQuery, GetRequestQueryResult?>
{
    public async Task<GetRequestQueryResult?> Handle(GetRequestQuery request, CancellationToken cancellationToken)
    {
        Response<GetRequestQueryResult> response = await _providerRelationshipsApiRestClient.GetRequest(request.RequestId, cancellationToken);

        return response.ResponseMessage.IsSuccessStatusCode ? response.GetContent() : null;
    }
}
