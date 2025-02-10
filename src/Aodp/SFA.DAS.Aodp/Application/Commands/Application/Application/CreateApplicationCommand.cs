using MediatR;

public class CreateApplicationCommand : IRequest<BaseMediatrResponse<CreateApplicationCommandResponse>>
{
    public string Title { get; set; }
    public string Owner { get; set; }
    public Guid FormVersionId { get; set; }
}
