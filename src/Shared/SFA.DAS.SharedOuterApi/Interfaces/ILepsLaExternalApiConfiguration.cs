namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface ILepsLaExternalApiConfiguration : IApiConfiguration
    {
        string Identifier { get; set; }
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        string Scope { get; set; }
    }
}