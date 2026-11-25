namespace SFA.DAS.LearnerData.Services.ShortCourses;

public class InvalidCourseException : Exception
{
    public InvalidCourseException(string message) : base(message) { }
}
