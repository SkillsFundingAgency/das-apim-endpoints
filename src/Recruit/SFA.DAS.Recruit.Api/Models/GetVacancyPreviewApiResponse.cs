using System.Collections.Generic;
using SFA.DAS.Recruit.Application.Queries.GetVacancyPreview;
using SFA.DAS.Recruit.Domain;

namespace SFA.DAS.Recruit.Api.Models;

public class GetVacancyPreviewApiResponse
{
    public string OverviewOfRole { get; set; }
    public List<string> Skills { get; set; }
    public List<string> CoreDuties { get; set; }
    public string StandardPageUrl { get; set; }
    public int Level { get; set; }
    public string Title { get; set; }
    public int EducationLevelNumber { get; set; }
    public ApprenticeshipLevel ApprenticeshipLevel { get; set; }
    public TrainingType ApprenticeshipType { get; set; }
    
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
            Title = source.Course.Title,
            ApprenticeshipLevel = ApprenticeshipLevelMapper.RemapFromInt(source.Course.Level),
            EducationLevelNumber = source.Course.Level,
            ApprenticeshipType = TrainingType.Standard
        };
    }

    
}