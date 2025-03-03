using MediatR;

namespace SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedbackAnnual
{
    public class GetProviderFeedbackAnnualQuery : IRequest<GetProviderFeedbackAnnualResult>
    {
        public int ProviderId { get; set; }
    }
}