using SFA.DAS.Aodp.Application;
using SFA.DAS.AODP.Domain.FormBuilder.Responses.Pages;

namespace SFA.DAS.AODP.Application.Queries.FormBuilder.Pages;

public class GetAllPagesQueryResponse : BaseResponse
{
    public List<Page> Data { get; set; }

    public class Page
    {
        public Guid Id { get; set; }
        public Guid SectionId { get; set; }
        public string Title { get; set; } = string.Empty;
        public Guid Key { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Order { get; set; }

        public static implicit operator Page(GetAllPagesApiResponse.Page entity)
        {
            return (new()
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                Order = entity.Order,
                SectionId = entity.SectionId,
                Key = entity.Key
            });
        }
    }

}