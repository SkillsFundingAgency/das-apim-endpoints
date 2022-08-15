namespace SFA.DAS.TrackProgress.Application.Models;

public record ProviderContext
{
	public long ProviderId { get; set; }
	public bool InSandboxMode { get; set; } 

	public override string ToString() => $"ProviderId {ProviderId} : InSandboxModel = {InSandboxMode}";

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
