using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Services;

public interface IBulkCourseMetadataService
{
    Task<Dictionary<string, int?>> GetOtjTrainingHoursForBulkUploadAsync(IEnumerable<string> courseCodes);
}