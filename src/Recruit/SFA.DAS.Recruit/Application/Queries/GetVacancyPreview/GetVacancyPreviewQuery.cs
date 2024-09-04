using MediatR;

namespace SFA.DAS.Recruit.Application.Queries.GetVacancyPreview;

public class GetVacancyPreviewQuery : IRequest<GetVacancyPreviewQueryResult>
{
    public int StandardId { get; set; }
}