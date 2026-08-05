using MediatR;

public class GetApplicationSectionByIdQuery : IRequest<BaseMediatrResponse<GetApplicationSectionByIdQueryResponse>>
{
    public GetApplicationSectionByIdQuery(Guid sectionId, Guid formVersionId)
    {
        SectionId = sectionId;
        FormVersionId = formVersionId;
    }
    public Guid SectionId { get; set; }
    public Guid FormVersionId { get; set; }

}
