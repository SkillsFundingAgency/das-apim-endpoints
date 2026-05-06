namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses
{
    public class GetRoutesListResponse
    {
        public IEnumerable<GetRoutesListItem> Routes { get; set; }   
    }
    
    public class GetRoutesListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}