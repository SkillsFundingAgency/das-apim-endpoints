using System.Collections.Generic;
using System.IO;

namespace SFA.DAS.RoatpCourseManagement.Services.NationalAchievementRates
{
    public interface IZipArchiveHelper
    {
        IEnumerable<T> ExtractModelFromCsvFileZipStream<T>(Stream stream, string filePath);
    }
}