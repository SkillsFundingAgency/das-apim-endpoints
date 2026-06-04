using System;
using MediatR;

namespace SFA.DAS.Approvals.Application.Cohorts.Queries.GetAssignAllowEmployerAdd;

public class GetAssignAllowEmployerAddQuery : IRequest<GetAssignAllowEmployerAddQueryResult?>
{
    public Guid ReservationId { get; set; }
}
