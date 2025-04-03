using MediatR;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Providers.GetProviderSummary;

public class GetProviderSummaryQuery : IRequest<GetProviderSummaryQueryResult>
{
    public int Ukprn { get; }

    public GetProviderSummaryQuery(int ukprn)
    {
        Ukprn = ukprn;
    }
}
