namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface IInnerApiConfiguration : IApiConfiguration
    {
        string Identifier { get; set; }
    }
}