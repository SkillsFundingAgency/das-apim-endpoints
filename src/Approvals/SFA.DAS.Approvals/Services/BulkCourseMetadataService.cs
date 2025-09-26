using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.Approvals.Services;

public class BulkCourseMetadataService(
    ICourseTypeRulesService courseTypeRulesService,
    ILogger<BulkCourseMetadataService> logger)
    : IBulkCourseMetadataService
{
    public async Task<Dictionary<string, int?>> GetOtjTrainingHoursForBulkUploadAsync(IEnumerable<string> courseCodes)
    {
        var uniqueCourseCodes = courseCodes.Distinct().ToList();
        var otjTrainingHoursTasks = uniqueCourseCodes.Select(async courseCode =>
        {
            try
            {
                var rplRules = await courseTypeRulesService.GetRplRulesAsync(courseCode);
                return new { CourseCode = courseCode, OtjTrainingHours = rplRules.RplRules?.OffTheJobTrainingMinimumHours };
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to fetch RPL rules for course code {CourseCode}", courseCode);
                return new { CourseCode = courseCode, OtjTrainingHours = (int?)null };
            }
        });

        var results = await Task.WhenAll(otjTrainingHoursTasks);
            
        return results.ToDictionary(r => r.CourseCode, r => r.OtjTrainingHours);
    }
}