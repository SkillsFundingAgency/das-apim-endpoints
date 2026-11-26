using System.Net;

namespace SFA.DAS.LearnerData.Services;

// SLD has no English and Maths value for this course on this start date, so the submission can't be processed as sent
public class EnglishAndMathsValueNotFoundException(string learnAimRef, DateTime startDate)
    : Exception($"SLD has no English and Maths value for course {learnAimRef} starting {startDate:yyyy-MM-dd}.")
{
    public string LearnAimRef { get; } = learnAimRef;
    public DateTime StartDate { get; } = startDate;
}

// The English and Maths value could not be obtained from SLD for any reason other than it not existing
public class EnglishAndMathsValueUnavailableException : Exception
{
    public HttpStatusCode? StatusCode { get; }

    public EnglishAndMathsValueUnavailableException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
    }

    public EnglishAndMathsValueUnavailableException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
