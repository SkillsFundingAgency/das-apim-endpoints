﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Application.Queries.Sectors
{
    public class GetSectorsQueryHandler : IRequestHandler<GetSectorsQuery, GetSectorsQueryResult>
    {
        private readonly ICourseService _courseService;

        public GetSectorsQueryHandler (ICourseService courseService)
        {
            _courseService = courseService;
        }
        public async Task<GetSectorsQueryResult> Handle(GetSectorsQuery request, CancellationToken cancellationToken)
        {
            var response = await _courseService.GetRoutes();
            
            return new GetSectorsQueryResult
            {
                Sectors = response.Routes
            };
        }
    }
}