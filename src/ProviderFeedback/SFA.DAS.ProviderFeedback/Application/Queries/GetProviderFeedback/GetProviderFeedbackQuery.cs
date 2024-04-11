using MediatR;

namespace SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedback
{
    public class GetProviderFeedbackQuery : IRequest<GetProviderFeedbackResult>
    {
        public int ProviderId { get; set; }
    }
}