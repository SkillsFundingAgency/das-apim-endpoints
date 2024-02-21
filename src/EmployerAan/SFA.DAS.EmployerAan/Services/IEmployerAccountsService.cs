using SFA.DAS.EmployerAan.Models;

namespace SFA.DAS.EmployerAan.Services;

public interface IEmployerAccountsService
{
    Task<IEnumerable<EmployerAccountUser>> GetEmployerAccounts(EmployerProfile employerProfile, CancellationToken cancellationToken);
    Task<EmployerProfile> PutEmployerAccount(EmployerProfile employerProfile, CancellationToken cancellationToken);
}