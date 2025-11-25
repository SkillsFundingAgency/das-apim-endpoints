
namespace SFA.DAS.Approvals.Enums;

// Enum of Email overlapstatus
public enum OverlapStatus : short
{
    None = 0,
    OverlappingStartDate = 1,
    OverlappingEndDate = 2,
    DateEmbrace = 3,
    DateWithin = 4
}