using SFA.DAS.Aodp.Application;
using SFA.DAS.AODP.Domain.FormBuilder.Responses.Forms;

public class GetFormVersionByIdQueryResponse : BaseResponse
{
    public FormVersion? Data { get; set; }

    public class FormVersion
    {
        public Guid Id { get; set; }
        public Guid FormId { get; set; }
        public string Title { get; set; }
        public DateTime Version { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public List<Section> Sections { get; set; }

        public static implicit operator FormVersion(GetFormVersionByIdApiResponse.FormVersion formVersion)
        {
            return new FormVersion()
            {
                Id = formVersion.Id,
                FormId = formVersion.FormId,
                Description = formVersion.Description,
                Order = formVersion.Order,
                Title = formVersion.Title,
                Version = formVersion.Version,
                Status = formVersion.Status,
                Sections = [.. formVersion.Sections]

            };
        }
    }

    public class Section
    {
        public Guid Id { get; set; }
        public Guid Key { get; set; }
        public int Order { get; set; }
        public string Title { get; set; }

        public static implicit operator Section(GetFormVersionByIdApiResponse.Section section)
        {
            return new()
            {
                Id = section.Id,
                Key = section.Key,
                Order = section.Order,
                Title = section.Title
            };
        }
    }



}