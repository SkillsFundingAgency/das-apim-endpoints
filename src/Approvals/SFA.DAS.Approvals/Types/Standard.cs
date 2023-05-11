namespace SFA.DAS.Approvals.Types
{
    public class Standard
    {
        public Standard(string courseCode, string name)
        {
            CourseCode = courseCode;
            Name = name;
        }

        public string CourseCode { get; }
        public string Name { get; }
    }
}
