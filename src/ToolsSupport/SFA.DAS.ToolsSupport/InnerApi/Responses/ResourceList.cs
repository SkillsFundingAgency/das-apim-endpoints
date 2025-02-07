namespace SFA.DAS.ToolsSupport.InnerApi.Responses;

public class ResourceList : List<ResourceViewModel>
{
    public ResourceList(IEnumerable<ResourceViewModel> resources)
    {
        AddRange(resources);
    }
}
