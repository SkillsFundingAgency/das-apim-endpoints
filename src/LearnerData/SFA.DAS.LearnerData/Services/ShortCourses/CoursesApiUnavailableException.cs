namespace SFA.DAS.LearnerData.Services.ShortCourses;

public class CoursesApiUnavailableException : Exception
{
    public CoursesApiUnavailableException(string message) : base(message) { }
    public CoursesApiUnavailableException(string message, Exception inner) : base(message, inner) { }
}
