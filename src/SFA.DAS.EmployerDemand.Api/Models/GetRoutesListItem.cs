namespace SFA.DAS.EmployerDemand.Api.Models
{
    public class GetRoutesListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static implicit operator GetRoutesListItem(SharedOuterApi.InnerApi.Responses.GetRoutesListItem source)
        {
            return new GetRoutesListItem
            {
                Id = source.Id,
                Name = source.Name
            };
        }
    }
}