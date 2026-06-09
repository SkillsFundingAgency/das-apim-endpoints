using System;
using MediatR;

namespace SFA.DAS.RoatpCourseManagement.Application.UkrlpData.Queries.GetUkrlpProviders;

public record GetUkrlpProvidersQuery(DateTime? UpdatedSinceDate) : IRequest<GetUkrlpProvidersQueryResult>;
