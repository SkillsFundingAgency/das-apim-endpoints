using NServiceBus;

// ReSharper disable once CheckNamespace - consistent cross project naming
namespace SFA.DAS.Recruit.Jobs.NServiceBus.Commands;

public sealed record PublishVacancyCommand(Guid VacancyId) : ICommand;