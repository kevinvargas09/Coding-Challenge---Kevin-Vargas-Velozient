using DroneTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneTest.Service
{
    public interface IServiceLocation
    {
        public List<Trip> CalculateTrips(Drone drone, List<Location> locations);
        public List<Trip> Highlight(List<Trip> trips, bool descending);
        public string CreateFile(string content);
    }
}
