using MediatR;

namespace SFA.DAS.VacanciesManage.Application.TrainingCourses.Queries;

public record GetTrainingCoursesQuery(int? Ukprn = null) : IRequest<GetTrainingCoursesQueryResult>;