using DroneTest.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DroneTest.Service
{
    public class ServiceLocation : IServiceLocation
    {
        private readonly IHostingEnvironment environment;

        public ServiceLocation(IHostingEnvironment hostingEnvironment)
        {
            this.environment = hostingEnvironment;
        }

        public List<Trip> CalculateTrips(Drone drone, List<Location> locations)
        {
            List<Trip> tripsTotal = new List<Trip>();
            List<string> tripsDone = new List<string>();

            do
            {
                List<string> trips = new List<string>();
                int available = drone.Weight;

                foreach (Location location in locations.Where(x => !tripsDone.Contains(x.Name)).ToList())
                {
                    if (location.Weight <= available)
                    {
                        if ((available - location.Weight) >= 0)
                        {
                            trips.Add(location.Name);
                            tripsDone.Add(location.Name);
                            available = available - location.Weight;
                        }
                    }
                }

                Trip trip = new Trip();
                trip.Dron = drone.Name;
                trip.Locations = trips;
                trip.NumberOfTrips = trips.Count;
                tripsTotal.Add(trip); ;

            } while (locations.Where(x => !tripsDone.Contains(x.Name)).ToList().Count > 0);

            return tripsTotal;
        }

        public string CreateFile(string content)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();

            string fileName = configuration["param:file"] + ".txt";
            string filePath = Path.Combine(environment.ContentRootPath, configuration["param:file"], fileName);

            FileInfo fi = new FileInfo(filePath);

            try
            {
                if (fi.Exists)
                    fi.Delete();

                using (StreamWriter sw = fi.CreateText())
                {
                    sw.WriteLine(content);
                }

                using (StreamReader sr = File.OpenText(filePath))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }

                return filePath;
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
                return string.Empty;
            }
        }

        public List<Trip> Highlight(List<Trip> trips, bool descending)
        {
            if (descending)
                return trips.OrderByDescending(x => x.NumberOfTrips).ToList();
            else
                return trips.OrderBy(x => x.NumberOfTrips).ToList();
        }
    }
}
