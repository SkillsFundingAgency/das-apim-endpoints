using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Application.Application;

public class GetApplicationMessagesByApplicationIdQuery : IRequest<BaseMediatrResponse<GetApplicationMessagesByApplicationIdQueryResponse>>
{
    public Guid ApplicationId { get; set; }
    public string UserType { get; set; }
    public GetApplicationMessagesByApplicationIdQuery(Guid applicationId, string userType)
    {
        ApplicationId = applicationId;
        UserType = userType;
    }
}