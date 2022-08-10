using System.Runtime.Serialization;

namespace SFA.DAS.TrackProgress.Application.Models;

public class InvalidUkprnException : Exception
{
    public InvalidUkprnException(string invalidUkprn) : base($"`{invalidUkprn}` is not a valid UKPRN")
    {
    }

    public InvalidUkprnException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected InvalidUkprnException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}