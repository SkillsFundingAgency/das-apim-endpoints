﻿using MediatR;

namespace SFA.DAS.EmployerPR.Application.Permissions.Queries.GetPermissions;
public class GetPermissionsQuery : IRequest<GetPermissionsResponse>
{
    public long? Ukprn { get; set; }
    public int? AccountLegalEntityId { get; set; }
}