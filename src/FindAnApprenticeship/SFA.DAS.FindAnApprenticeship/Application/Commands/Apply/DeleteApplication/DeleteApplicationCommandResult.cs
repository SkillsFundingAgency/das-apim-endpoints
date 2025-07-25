namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteApplication;

public record DeleteApplicationCommandResult(string? EmployerName = null, string? VacancyTitle = null)
{
    public static readonly DeleteApplicationCommandResult None = new();
}