using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;

namespace SFA.DAS.LearnerData.Application.Fm36;

public static class AcademicYearFallbackHelper
{
    public static GetAcademicYearsResponse GetFallbackAcademicYearResponse(int academicYear)
    {
        // Parse the first two digits as the start year
        var startYear = 2000 + int.Parse(academicYear.ToString()[..2]);
        var endYear = startYear + 1;

        return new GetAcademicYearsResponse
        {
            AcademicYear = $"{academicYear}",
            StartDate = new DateTime(startYear, 8, 1),
            EndDate = new DateTime(endYear, 7, 31),
            HardCloseDate = null // unused and unsafe to predict
        };
    }
}
