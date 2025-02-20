using System.Reflection;

namespace SFA.DAS.Earnings.Api.Learnerdata;

public class ResourceLoader
{
    public string LoadEmbeddedResource(string resourcePath)
    {
        // Get the current assembly
        var assembly = Assembly.GetExecutingAssembly();
        
        // Ensure the resource exists in the assembly
        using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
        {
            if (stream == null)
            {
                throw new FileNotFoundException("Resource not found.", resourcePath);
            }
            
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}