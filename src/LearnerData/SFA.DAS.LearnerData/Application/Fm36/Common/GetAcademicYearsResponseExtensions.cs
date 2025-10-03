using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;

namespace SFA.DAS.LearnerData.Application.Fm36.Common;

public static class GetAcademicYearsResponseExtensions
{
    public static short GetShortAcademicYear(this GetAcademicYearsResponse response)
    {
        if (!short.TryParse(response.AcademicYear, out var result))
            throw new ArgumentException("Failed to parse year from academic year response", nameof(response));

        return result;
    }
}
