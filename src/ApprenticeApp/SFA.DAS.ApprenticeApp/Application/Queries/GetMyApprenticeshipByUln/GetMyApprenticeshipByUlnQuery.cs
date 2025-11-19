using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Queries.GetMyApprenticeshipByUln
{
    public class GetMyApprenticeshipByUlnQuery : IRequest<GetMyApprenticeshipByUlnQueryResult>
    {
        public int Uln { get; set; }
    }
}
