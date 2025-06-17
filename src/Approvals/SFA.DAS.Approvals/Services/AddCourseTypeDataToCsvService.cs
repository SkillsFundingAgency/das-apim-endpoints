using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Requests;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Services;

public interface IAddCourseTypeDataToCsvService
{
    public Task<List<BulkUploadAddDraftApprenticeshipExtendedRequest>> PopulateWithCourseTypeData(List<BulkUploadAddDraftApprenticeshipRequest> csvRecords);
}

public class AddCourseTypeDataToCsvService(
    ICourseTypesApiClient courseTypesApiClient,
    IMapper mapper,
    ILogger<AddCourseTypeDataToCsvService> logger)
    : IAddCourseTypeDataToCsvService
{
    private List<CourseTypeData> _courseTypeData = [];

    public async Task<List<BulkUploadAddDraftApprenticeshipExtendedRequest>> PopulateWithCourseTypeData(List<BulkUploadAddDraftApprenticeshipRequest> csvRecords)
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
            logger.LogInformation($"Fetching course type data for course code {courseCode}");
            var response = await courseTypesApiClient.GetWithResponseCode<GetLearnerAgeResponse>(new GetLearnerAgeRequest(courseCode));

            courseTypeData = new CourseTypeData
            {
                CourseCode = courseCode,
                MinimumAgeAtApprenticeshipStart = response.Body.MinimumAge,
                MaximumAgeAtApprenticeshipStart = response.Body.MaximumAge
            };
            _courseTypeData.Add(courseTypeData);
        };
        return courseTypeData;
    }

    public class CourseTypeData
    {
        public string CourseCode { get; set; }
        public int? MinimumAgeAtApprenticeshipStart { get; set; }
        public int? MaximumAgeAtApprenticeshipStart { get; set; }
    }
}