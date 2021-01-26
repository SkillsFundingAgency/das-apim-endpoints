namespace SFA.DAS.FindEpao.Api.Models
{
    public class GetDeliveryAreaListItem
    {
        public int Id { get; set; }
        public string Area { get; set; }
        public string Status { get; set; }
        public int Ordering { get; set; }

        public static implicit operator GetDeliveryAreaListItem(InnerApi.Responses.GetDeliveryAreaListItem source)
        {
            return new GetDeliveryAreaListItem
            {
                Id = source.Id,
                Area = source.Area,
                Status = source.Status,
                Ordering = source.Ordering
            };
        }
    }
}