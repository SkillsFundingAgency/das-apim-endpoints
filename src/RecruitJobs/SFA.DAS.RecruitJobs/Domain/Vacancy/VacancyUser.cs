namespace SFA.DAS.RecruitJobs.Domain.Vacancy;

public class VacancyUser
{
    public string UserId { get; set; }
    public string DfEUserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public long? Ukprn { get; set; }
}