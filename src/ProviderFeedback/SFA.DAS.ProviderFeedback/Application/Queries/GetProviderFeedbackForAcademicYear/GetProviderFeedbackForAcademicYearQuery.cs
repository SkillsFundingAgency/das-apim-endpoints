using MediatR;

namespace SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedbackForAcademicYear
{
    public class GetProviderFeedbackForAcademicYearQuery : IRequest<GetProviderFeedbackForAcademicYearResult>
    {
        public int ProviderId { get; set; }
        public string Year { get; set; }
    }
}