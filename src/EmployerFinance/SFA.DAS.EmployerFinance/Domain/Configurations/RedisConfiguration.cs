namespace SFA.DAS.EmployerFinance.Domain.Configurations;

public sealed record RedisConfiguration
{
    public required string ConnectionString { get; init; }
    public required int DefaultCacheExpirationInMinutes { get; init; } = 60;
}