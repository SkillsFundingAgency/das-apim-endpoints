namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderEvent
{
    public class PageOfResults<T>
    {
        public int PageNumber { get; set; }
        public int TotalNumberOfPages { get; set; }
        public T[] Items { get; set; }
    }
}
