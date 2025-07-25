using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.Queries.GetProvidersByLarsCode;

public record GetProvidersByLarsCodeQueryResult(IEnumerable<ProviderByLarsCodeItem> Providers);

public record ProviderByLarsCodeItem(string Name, int Ukprn);