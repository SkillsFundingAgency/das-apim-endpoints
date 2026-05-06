namespace SFA.DAS.Approvals.Types;

public class Standard(string courseCode, string name, int? level=null)
{
    public string CourseCode { get; } = courseCode;
    public string Name { get; } = name;
    public int? Level { get; } = level;
}