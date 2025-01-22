using MediatR;

namespace SFA.DAS.AODP.Application.Commands.FormBuilder.Sections;

public class UpdateSectionCommand : IRequest<UpdateSectionCommandResponse>
{
    public readonly Guid FormVersionId;
    public Section Data;

    public UpdateSectionCommand(Guid formVersionId, Section data)
    {
        FormVersionId = formVersionId;
        Data = data;
    }

    public class Section
    {
        public Guid Id { get; set; }
        public Guid FormVersionId { get; set; }
        public Guid Key { get; set; }
        public int Order { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? NextSectionId { get; set; }
    }
}
