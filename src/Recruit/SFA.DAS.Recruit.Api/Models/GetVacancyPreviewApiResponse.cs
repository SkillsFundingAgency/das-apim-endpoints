using System.Collections.Generic;
using SFA.DAS.Recruit.Application.Queries.GetVacancyPreview;

namespace SFA.DAS.Recruit.Api.Models;

public class GetVacancyPreviewApiResponse
{
    public string OverviewOfRole { get; set; }
    public List<string> Skills { get; set; }
    public List<string> CoreDuties { get; set; }
    public string StandardPageUrl { get; set; }
    public int Level { get; set; }
    public string Title { get; set; }
    
    public static implicit operator GetVacancyPreviewApiResponse(GetVacancyPreviewQueryResult source)
    {
        if (source.Course == null)
        {
            return null;
        }
        
        return new GetVacancyPreviewApiResponse
        {
            OverviewOfRole = source.Course.OverviewOfRole,
            StandardPageUrl = source.Course.StandardPageUrl,
            CoreDuties = source.Course.CoreDuties,
            Skills = source.Course.Skills,
            Level = source.Course.Level,
            Title = source.Course.Title
        };
    }
}