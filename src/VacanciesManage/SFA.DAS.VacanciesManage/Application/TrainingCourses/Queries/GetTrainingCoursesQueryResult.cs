using SFA.DAS.VacanciesManage.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.VacanciesManage.Application.TrainingCourses.Queries;

public class GetTrainingCoursesQueryResult
{
    public IEnumerable<GetStandardsListItem> TrainingCourses { get; set; }
}