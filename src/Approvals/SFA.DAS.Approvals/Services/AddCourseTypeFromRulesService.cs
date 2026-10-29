using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Services;

public interface IAddCourseTypeFromRulesService
{
    public Task<List<BulkUploadAddDraftApprenticeshipExtendedRequest>> MapAndAddCourseTypeData(List<BulkUploadAddDraftApprenticeshipRequest> csvRecords);
}

public class AddCourseTypeFromRulesService(
    ICourseTypeRulesService courseTypeRulesService,
    IMapper mapper,
    ILogger<AddCourseTypeFromRulesService> logger)
    : IAddCourseTypeFromRulesService
{
    private List<CourseTypeData> _courseTypeData = [];

    public async Task<List<BulkUploadAddDraftApprenticeshipExtendedRequest>> MapAndAddCourseTypeData(List<BulkUploadAddDraftApprenticeshipRequest> csvRecords)
    {
        var extended = new List<BulkUploadAddDraftApprenticeshipExtendedRequest>();
        logger.LogInformation("Adding course type data to CSV records");
        foreach (var item in csvRecords)
        {
            var extendedItem = mapper.Map<BulkUploadAddDraftApprenticeshipExtendedRequest>(item);
            var courseTypeData = await AddAndReturn(item.CourseCode);
            if (courseTypeData != null)
            {
                logger.LogInformation($"Course type data found for course code {item.CourseCode}: MinAge={courseTypeData.MinimumAgeAtApprenticeshipStart}, MaxAge={courseTypeData.MaximumAgeAtApprenticeshipStart}");
                extendedItem.MinimumAgeAtApprenticeshipStart = courseTypeData.MinimumAgeAtApprenticeshipStart;
                extendedItem.MaximumAgeAtApprenticeshipStart = courseTypeData.MaximumAgeAtApprenticeshipStart;
            }
            extended.Add(extendedItem);
        }
        return extended;
    }

    private async Task<CourseTypeData> AddAndReturn(string courseCode)
    {
        if (string.IsNullOrWhiteSpace(courseCode))
        {
            return null;
        }

        var courseTypeData = _courseTypeData.FirstOrDefault(x => x.CourseCode == courseCode);
        if (courseTypeData == null)
        {
            logger.LogInformation($"Fetching course type rules for course code {courseCode}");

            var rules = await courseTypeRulesService.GetCourseTypeRulesAsync(courseCode);

            courseTypeData = new CourseTypeData
            {
                CourseCode = courseCode,
                MinimumAgeAtApprenticeshipStart = rules.LearnerAgeRules.MinimumAge,
                MaximumAgeAtApprenticeshipStart = rules.LearnerAgeRules.MaximumAge
            };
            _courseTypeData.Add(courseTypeData);
        }
        
        return courseTypeData;
    }

    public class CourseTypeData
    {
        public string CourseCode { get; set; }
        public int? MinimumAgeAtApprenticeshipStart { get; set; }
        public int? MaximumAgeAtApprenticeshipStart { get; set; }
    }
}