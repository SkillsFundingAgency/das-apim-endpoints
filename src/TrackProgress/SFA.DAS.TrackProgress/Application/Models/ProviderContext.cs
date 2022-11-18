namespace SFA.DAS.TrackProgress.Application.Models;

public record ProviderContext
{
	public long ProviderId { get; private set; }
	public bool InSandboxMode { get; private set; } 

	public override string ToString() => $"ProviderId {ProviderId} : InSandboxMode = {InSandboxMode}";

	public static ProviderContext Create(string ukPrn, string? isSandbox)
	{
		if (long.TryParse(ukPrn, out var providerId) && providerId > 0)
		{
			var inSandboxMode = false;
			bool.TryParse(isSandbox, out inSandboxMode);
			return new() { ProviderId = providerId, InSandboxMode = inSandboxMode };
		}
        throw new InvalidUkprnException(ukPrn);
	}
}
