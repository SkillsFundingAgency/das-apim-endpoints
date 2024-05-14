using System;

namespace SFA.DAS.FindAnApprenticeship.Services;

public interface IVacancy
{
    string VacancyReference { get; }
    public string EmployerName { get; }
    public string Title { get; }
    public DateTime ClosingDate { get; }
    public DateTime? ClosedDate { get;}
    int CourseId { get; }
}