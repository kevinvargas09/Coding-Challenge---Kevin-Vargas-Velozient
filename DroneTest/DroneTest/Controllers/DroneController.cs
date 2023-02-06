using DroneTest.Model;
using DroneTest.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DroneController : Controller
    {
        private readonly IServiceLocation _serviceLocation;
        private const int maxSquad = 100;

        public DroneController(IServiceLocation serviceLocation)
        {
            _serviceLocation = serviceLocation;
        }

        [HttpPost("CalculateTrips")]
        public JsonResult Post([FromBody] List<Drone> drones)
        {
            try
            {
                string result = string.Empty;
                int countTrips = 0;

                if (drones.Count > maxSquad)
                {
                    return new JsonResult(new { success = false, result = "You have overcome drone limit squad of 100" });
                }
                else
                {
                    foreach (Drone drone in drones)
                    {
                        List<Trip> possibleTrips = _serviceLocation.CalculateTrips(drone, drone.Locations);
                        possibleTrips = _serviceLocation.Highlight(possibleTrips, true);

                        result = result + drone.Name + "\n";

                        foreach (Trip trip in possibleTrips)
                        {
                            countTrips++;
                            result = result + "Trip #" + countTrips.ToString() + "\n";
                            result = result + string.Join(",", trip.Locations) + "\n";
                        } 
                    }

                    string path = _serviceLocation.CreateFile(result);

                    if (!string.IsNullOrEmpty(path))
                        return new JsonResult(new { success = true, result = "Process finalised properly, you can find output file in the following path: " + path });
                    else
                        return new JsonResult(new { success = false, result = "Process finalised with errors" });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }
    }
}
