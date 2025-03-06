namespace SFA.DAS.ToolsSupport.Interfaces;

public interface ISecureTokenHttpClient
{
    Task<string> GetAsync(string url);
}
