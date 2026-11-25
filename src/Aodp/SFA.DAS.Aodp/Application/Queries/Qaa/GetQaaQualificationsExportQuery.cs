using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.Application.Queries.Qaa
{
    [ExcludeFromCodeCoverage]
    public class GetQaaQualificationsExportQuery : IRequest<BaseMediatrResponse<GetQaaQualificationsExportQueryResponse>>
    {
        public string CurrentUsername { get; set; } = string.Empty;
    }
}
