using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.Application.Queries.Qualifications;

[ExcludeFromCodeCoverage]
public class GetChangedQualificationsQuery : IRequest<BaseMediatrResponse<GetChangedQualificationsQueryResponse>>
{
    public string? Name { get; set; }
    public string? Organisation { get; set; }
    public string? QAN { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
    public string? ProcessStatusFilter { get; set; }
}
