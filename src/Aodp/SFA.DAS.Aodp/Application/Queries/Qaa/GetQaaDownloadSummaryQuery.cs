using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.Application.Queries.Qaa
{
    [ExcludeFromCodeCoverage]
    public class GetQaaDownloadSummaryQuery : IRequest<BaseMediatrResponse<GetQaaDownloadSummaryQueryResponse>>
    {
    }
}
