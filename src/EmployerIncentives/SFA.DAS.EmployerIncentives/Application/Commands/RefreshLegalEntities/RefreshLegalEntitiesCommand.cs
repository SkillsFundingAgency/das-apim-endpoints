using MediatR;

namespace SFA.DAS.EmployerIncentives.Application.Queries.GetLegalEntities
{
    public class RefreshLegalEntitiesCommand : IRequest<Unit>
    {
        public RefreshLegalEntitiesCommand(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageNumber{ get ; set ; }
        public int PageSize { get; set; }
    }
}