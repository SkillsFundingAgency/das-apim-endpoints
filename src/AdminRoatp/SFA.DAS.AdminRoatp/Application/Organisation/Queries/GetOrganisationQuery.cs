using MediatR;

namespace SFA.DAS.AdminRoatp.Application.Organisation.Queries;
public record GetOrganisationQuery(int Ukprn) : IRequest<GetOrganisationQueryResponse>;

public class GetOrganisationQueryResponse
{
    public Guid Id { get; set; }
    public int Ukprn { get; set; }
    public required string LegalName { get; set; }
    public string? TradingName { get; set; }
    public required string OrganisationStatus { get; set; }
    public DateTime StatusDate { get; set; }
}

public class GetOrganisationQueryHandler : IRequestHandler<GetOrganisationQuery, GetOrganisationQueryResponse>
{
    public Task<GetOrganisationQueryResponse> Handle(GetOrganisationQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new GetOrganisationQueryResponse() { LegalName = "organisation name", Ukprn = request.Ukprn, OrganisationStatus = "Active" });
    }
}