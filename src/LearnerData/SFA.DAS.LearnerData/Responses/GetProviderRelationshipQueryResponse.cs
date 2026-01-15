using SFA.DAS.LearnerData.Application.GetProviderRelationships;

namespace SFA.DAS.LearnerData.Responses;

public class GetProviderRelationshipQueryResponse
{    public required string UkPRN { get; set; }

    public int Type { get; set; }

    public int Status { get; set; }

    public EmployerDetails?[]? Employers { get; set; }
}
