using MediatR;
using SFA.DAS.Aodp.Application;

public class GetApplicationFormStatusByApplicationIdQuery : IRequest<BaseMediatrResponse<GetApplicationFormStatusByApplicationIdQueryResponse>>
{
    public GetApplicationFormStatusByApplicationIdQuery(Guid formVersionId, Guid applicationId)
    {
        FormVersionId = formVersionId;
        ApplicationId = applicationId;
    }
    public Guid FormVersionId { get; set; }
    public Guid ApplicationId { get; set; }

}
