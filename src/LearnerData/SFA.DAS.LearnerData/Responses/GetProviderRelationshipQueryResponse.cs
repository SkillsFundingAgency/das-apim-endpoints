using SFA.DAS.LearnerData.Application.GetProviderRelationships;

namespace SFA.DAS.LearnerData.Responses;

public class GetProviderRelationshipQueryResponse
{
    public string UkPRN { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public EmployerDetails[] Employers { get; set; }
}