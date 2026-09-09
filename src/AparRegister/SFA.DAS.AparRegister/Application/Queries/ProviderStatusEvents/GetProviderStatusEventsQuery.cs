using System.Collections.Generic;
using MediatR;
using SFA.DAS.AparRegister.InnerApi.Responses;

namespace SFA.DAS.AparRegister.Application.Queries.ProviderStatusEvents;

public record GetProviderStatusEventsQuery(int SinceEventId, int PageSize, int PageNumber) : IRequest<IEnumerable<ProviderStatusEvent>>;
