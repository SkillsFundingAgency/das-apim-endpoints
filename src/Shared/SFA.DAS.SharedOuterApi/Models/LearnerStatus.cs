namespace SFA.DAS.SharedOuterApi.Models;

//PR DISCUSSION POINT: should we put this somewhere else?
public enum LearnerStatus
{
    None,
    WaitingToStart,
    InLearning,
    BreakInLearning,
    Withdrawn,
    Completed
}