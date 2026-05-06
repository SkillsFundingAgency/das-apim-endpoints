using MediatR;

public class EditApplicationCommand : IRequest<BaseMediatrResponse<EditApplicationCommandResponse>>
{
    public string? QualificationNumber { get; set; }
    public string Title { get; set; }
    public string Owner { get; set; }
    public Guid ApplicationId { get; set; }
}

