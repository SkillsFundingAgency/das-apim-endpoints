using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetLocations
{
    public class GetLocationQuery : IRequest<GetLocationResult>
    {
        public string Location { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public GetLocationQuery(string location)
        {
            Location = location;
        }
    }
}
