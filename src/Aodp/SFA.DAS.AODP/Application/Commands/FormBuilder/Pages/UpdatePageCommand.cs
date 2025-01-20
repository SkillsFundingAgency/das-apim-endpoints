using MediatR;

namespace SFA.DAS.AODP.Application.Commands.FormBuilder.Pages;

public class UpdatePageCommand : IRequest<UpdatePageCommandResponse>
{
    public readonly Guid PageId;
    public readonly Page Data;

    public UpdatePageCommand(Guid pageId, Page data)
    {
        PageId = pageId;
        Data = data;
    }

    public class Page
    {
        public Guid Id { get; set; }
        public Guid SectionId { get; set; }
        public string Title { get; set; }
        public Guid Key { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public int? NextPageId { get; set; }
    }
}
