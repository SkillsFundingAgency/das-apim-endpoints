using System;
using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.RoatpCourseManagement.Application.UkrlpData.Queries.GetUkrlpProviders;

public class GetUkrlpProvidersQuery : IRequest<GetUkrlpProvidersQueryResult>
{
    public IEnumerable<int> Ukprns { get; set; } = [];
    public DateTime? UpdatedSinceDate { get; set; }
}
