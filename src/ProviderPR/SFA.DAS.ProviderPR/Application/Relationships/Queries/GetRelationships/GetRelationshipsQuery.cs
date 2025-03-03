using MediatR;
using SFA.DAS.ProviderPR.InnerApi.Requests;
using SFA.DAS.ProviderPR.InnerApi.Responses;

namespace SFA.DAS.ProviderPR.Application.Relationships.Queries.GetRelationships;

public class GetRelationshipsQuery : IRequest<GetProviderRelationshipsResponse>
{
    public long Ukprn { get; set; }
    public GetProviderRelationshipsRequest Request { get; set; }

    public GetRelationshipsQuery(long ukprn, GetProviderRelationshipsRequest request)
    {
        Ukprn = ukprn;
        Request = request;
    }
}
