using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.Responses;

public class GetStandardsListItemResponse
{
    public int LarsCode { get; set; }
    public string Title { get; set; }
    public int Level { get; set; }
    
    public string OverviewOfRole { get; set; }


    public List<string> CoreSkills => GetCoreSkillsCount();

    public string StandardPageUrl { get; set; }

    public List<string> Skills { get; set; }
    public bool CoreAndOptions { get; set; }
    public List<string> CoreDuties { get; set; }
    private List<string> GetCoreSkillsCount()
    {
        if (CoreAndOptions)
        {
            return CoreDuties;
        }
        return  Skills;
    }
}