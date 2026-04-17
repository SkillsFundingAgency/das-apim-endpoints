using NServiceBus;

// ReSharper disable once CheckNamespace -- THIS MUST STAY LIKE THIS TO MATCH THE EVENT FROM RECRUIT JOBS
namespace SFA.DAS.Recruit.Jobs.NServiceBus.Commands;

public sealed record PublishVacancyCommand(Guid VacancyId) : ICommand;