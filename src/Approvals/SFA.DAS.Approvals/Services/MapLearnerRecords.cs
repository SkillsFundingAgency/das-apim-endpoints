using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.Learners.Queries;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.Courses;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Services;

public interface IMapLearnerRecords
{
    List<LearnerSummary> Map(List<LearnerDataRecord> data, List<GetAllStandardsResponse.TrainingProgramme> trainingProgrammes);
    List<Course> PopulateMissingTrainingNames(List<Course> data, List<GetAllStandardsResponse.TrainingProgramme> trainingProgrammes);
}



public class MapLearnerRecords(ILogger<IMapLearnerRecords> logger) : IMapLearnerRecords
{
    public List<LearnerSummary> Map(List<LearnerDataRecord> learners, List<GetAllStandardsResponse.TrainingProgramme> trainingProgrammes)
    {
        logger.LogInformation("Mapping Learner record to summary");

        return learners.ConvertAll(learner =>
            {
                var courseName = learner.TrainingName;

                if (string.IsNullOrEmpty(courseName))
                {
                    var matchingProgramme = trainingProgrammes.FirstOrDefault(p => p.CourseCode == learner.TrainingCode);

                    courseName = matchingProgramme?.Name;
                }

                return new LearnerSummary
                {
                    Id = learner.Id,
                    FirstName = learner.FirstName,
                    LastName = learner.LastName,
                    Uln = learner.Uln,
                    Course = courseName,
                    StartDate = learner.StartDate,
                    LearningType = learner.LearningType
                };
            });
    }

    public List<Course> PopulateMissingTrainingNames(List<Course> courses, List<GetAllStandardsResponse.TrainingProgramme> trainingProgrammes)
    {
        logger.LogInformation("Populating Learner courses with any missing training names");

        return courses.ConvertAll(course =>
        {
            var courseName = course.TrainingName;

            if (string.IsNullOrEmpty(courseName))
            {
                var matchingProgramme = trainingProgrammes.FirstOrDefault(p => p.CourseCode == course.TrainingCode);

                courseName = matchingProgramme?.Name;
            }

            return new Course
            {
                TrainingCode = course.TrainingCode,
                TrainingName = courseName
            };
        });
    }


}