using System;

namespace SFA.DAS.Approvals.Exceptions;

public enum PreviousOrCurrentAcademicYear
{
    Current,
    Previous
}

public class AcademicYearDataIncompleteException : Exception
{
    public AcademicYearDataIncompleteException(PreviousOrCurrentAcademicYear previousOrCurrentAcademicYear) : base($"Incomplete academic year information for the {previousOrCurrentAcademicYear} academic year")
    {
    }
}