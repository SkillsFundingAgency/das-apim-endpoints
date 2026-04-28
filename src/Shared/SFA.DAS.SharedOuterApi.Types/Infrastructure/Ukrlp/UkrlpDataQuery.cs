using MediatR;

namespace SFA.DAS.SharedOuterApi.Types.Infrastructure.Ukrlp;

public class UkrlpDataQuery : IRequest<GetUkrlpDataQueryResponse>
{
    public List<long> Ukprns { get; set; }
    public DateTime? ProvidersUpdatedSince { get; set; }
}