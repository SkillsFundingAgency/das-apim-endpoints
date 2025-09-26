namespace SFA.DAS.Approvals.Types;

public class Standard(string courseCode, string name)
{
    public string CourseCode { get; } = courseCode;
    public string Name { get; } = name;
}