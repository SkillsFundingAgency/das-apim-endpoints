namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface ITokenPassThroughApiConfiguration : IInternalApiConfiguration
    {
        string BearerTokenSigningKey { get; set; }
    }
}
