namespace SFA.DAS.ToolsSupport.Interfaces;
public interface IPayRefHashingService
{
    string HashValue(string id);
    string DecodeValueToString(string id);
}